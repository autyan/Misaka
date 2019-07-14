using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Misaka.Extensions.Json
{
    public class CustomContractResolver : DefaultContractResolver
    {
        private readonly string[] _ignoreProperties;
        private readonly bool _lowerCase;
        private readonly bool _serializeNonPublic;

        public CustomContractResolver(bool serializeNonPublic, bool lowerCase, params string[] ignoreProperties)
        {
            _serializeNonPublic = serializeNonPublic;
            _lowerCase = lowerCase;
            _ignoreProperties = ignoreProperties;
        }

        protected override string ResolvePropertyName(string propertyName)
        {
            return _lowerCase ? propertyName.ToLower() : propertyName;
        }

        protected override IList<JsonProperty> CreateProperties(Type type, MemberSerialization memberSerialization)
        {
            var properties = base.CreateProperties(type, memberSerialization);
            if (_ignoreProperties.Length > 0)
            {
                properties = properties.Where(p => !_ignoreProperties.Contains(p.PropertyName))
                                       .ToList();
            }
            return properties;
        }

        protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
        {
            var prop = base.CreateProperty(member, memberSerialization);

            if (_serializeNonPublic)
            {
                //TODO: Maybe cache

                if (!prop.Writable)
                {
                    var property = member as PropertyInfo;
                    if (property != null)
                    {
                        var hasPrivateSetter = property.GetSetMethod(true) != null;
                        prop.Writable = hasPrivateSetter;
                    }
                }
            }

            return prop;
        }
    }
}
