using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Misaka.Extensions.Json
{
    public class IgnoreInvalidStringEnumConverter : StringEnumConverter
    {
        public override object ReadJson(JsonReader     reader,
                                        Type           objectType,
                                        object         existingValue,
                                        JsonSerializer serializer)
        {
            try
            {
                return base.ReadJson(reader, objectType, existingValue, serializer);
            }
            catch (Exception)
            {
                if (objectType.IsGenericType && objectType.GetGenericTypeDefinition() == typeof(Nullable<>))
                    return null;

                return 0;
            }
        }
    }
}
