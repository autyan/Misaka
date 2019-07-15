using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;

namespace Misaka.Extensions.Json
{
    public static class JsonHelper
    {
        private static readonly ConcurrentDictionary<string, JsonSerializerSettings> SettingDictionary = new ConcurrentDictionary<string, JsonSerializerSettings>();

        internal static JsonSerializerSettings InternalGetCustomJsonSerializerSettings(bool serializeNonPublic,
                                                                                       bool loopSerialize,
                                                                                       bool useCamelCase,
                                                                                       bool useStringEnumConvert,
                                                                                       bool ignoreSerializableAttribute,
                                                                                       bool ignoreNullValue,
                                                                                       bool lowerCase)
        {
            var customSettings = new JsonSerializerSettings
            {
                ContractResolver = new CustomContractResolver(serializeNonPublic, lowerCase)
            };

            if (loopSerialize)
            {
                customSettings.PreserveReferencesHandling = PreserveReferencesHandling.Objects;
                customSettings.ReferenceLoopHandling = ReferenceLoopHandling.Serialize;
            }
            else
            {
                customSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            }
            if (useStringEnumConvert)
            {
                customSettings.Converters.Add(new IgnoreInvalidStringEnumConverter());
            }

            ((DefaultContractResolver)customSettings.ContractResolver).IgnoreSerializableAttribute = ignoreSerializableAttribute;

            if (useCamelCase)
            {
                if (customSettings.ContractResolver is DefaultContractResolver resolver)
                {
                    resolver.NamingStrategy = new CamelCaseNamingStrategy
                    {
                        ProcessDictionaryKeys = true,
                        OverrideSpecifiedNames = true
                    };
                }
            }
            if (ignoreNullValue)
            {
                customSettings.NullValueHandling = NullValueHandling.Ignore;
            }
            return customSettings;
        }

        public static JsonSerializerSettings GetCustomJsonSerializerSettings(bool serializeNonPublic,
                                                                             bool loopSerialize,
                                                                             bool useCamelCase,
                                                                             bool useStringEnumConvert = true,
                                                                             bool ignoreSerializableAttribute = true,
                                                                             bool ignoreNullValue = true,
                                                                             bool lowerCase = false,
                                                                             bool useCached = true)
        {

            JsonSerializerSettings settings;
            if (useCached)
            {
                var key = $"{Convert.ToInt16(serializeNonPublic)}{Convert.ToInt16(loopSerialize)}{Convert.ToInt16(useCamelCase)}{Convert.ToInt16(useStringEnumConvert)}";
                key += $"{Convert.ToInt16(ignoreSerializableAttribute)}{Convert.ToInt16(ignoreNullValue)}{Convert.ToInt16(lowerCase)}";
                settings = SettingDictionary.GetOrAdd(key,
                                                      k => InternalGetCustomJsonSerializerSettings(serializeNonPublic,
                                                                                                   loopSerialize,
                                                                                                   useCamelCase,
                                                                                                   useStringEnumConvert,
                                                                                                   ignoreSerializableAttribute,
                                                                                                   ignoreNullValue,
                                                                                                   lowerCase));
            }
            else
            {
                settings = InternalGetCustomJsonSerializerSettings(serializeNonPublic,
                                                                   loopSerialize,
                                                                   useCamelCase,
                                                                   useStringEnumConvert,
                                                                   ignoreSerializableAttribute,
                                                                   ignoreNullValue,
                                                                   lowerCase);
            }
            return settings;
        }

        public static string ToJson(this object obj,
                                    JsonSerializerSettings settings = null)
        {
            return settings == null 
                       ? ToJson(obj, false) 
                       : JsonConvert.SerializeObject(obj, settings);
        }

        internal static string ToJson(this object obj,
                                    bool serializeNonPublic = false,
                                    bool loopSerialize = false,
                                    bool useCamelCase = false,
                                    bool ignoreNullValue = true,
                                    bool useStringEnumConvert = true)
        {
            return JsonConvert.SerializeObject(obj,
                                               GetCustomJsonSerializerSettings(serializeNonPublic, loopSerialize, useCamelCase, useStringEnumConvert, ignoreNullValue: ignoreNullValue));
        }

        public static object ToObject(this string json,
                                          bool serializeNonPublic = false,
                                          bool loopSerialize = false,
                                          bool useCamelCase = false)
        {

            if (string.IsNullOrWhiteSpace(json))
            {
                return null;
            }
            return JsonConvert.DeserializeObject(json,
                                                 GetCustomJsonSerializerSettings(serializeNonPublic, loopSerialize, useCamelCase));

        }

        public static object ToObject(this string json,
                                          Type jsonType,
                                          bool serializeNonPublic = false,
                                          bool loopSerialize = false,
                                          bool useCamelCase = false)
        {
            if (string.IsNullOrWhiteSpace(json))
            {
                return null;
            }
            if (jsonType == typeof(List<dynamic>))
            {
                return json.ToDynamicObjects(serializeNonPublic, loopSerialize, useCamelCase);
            }
            if (jsonType == typeof(object))
            {
                return json.ToDynamicObject(serializeNonPublic, loopSerialize, useCamelCase);
            }
            return JsonConvert.DeserializeObject(json,
                                                 jsonType,
                                                 GetCustomJsonSerializerSettings(serializeNonPublic,
                                                                                 loopSerialize,
                                                                                 useCamelCase));

        }

        public static T ToObject<T>(this string json,
                                        bool serializeNonPublic = false,
                                        bool loopSerialize = false,
                                        bool useCamelCase = false)
        {
            if (string.IsNullOrWhiteSpace(json))
            {
                return default(T);
            }
            if (typeof(T) == typeof(List<dynamic>))
            {
                return (T)json.ToDynamicObjects(serializeNonPublic,
                                                 loopSerialize,
                                                 useCamelCase);
            }
            if (typeof(T) == typeof(object))
            {
                return json.ToDynamicObject(serializeNonPublic,
                                            loopSerialize,
                                            useCamelCase);
            }
            return JsonConvert.DeserializeObject<T>(json,
                                                    GetCustomJsonSerializerSettings(serializeNonPublic,
                                                                                    loopSerialize,
                                                                                    useCamelCase));

        }

        public static dynamic ToDynamicObject(this string json,
                                              bool serializeNonPublic = false,
                                              bool loopSerialize = false,
                                              bool useCamelCase = false)
        {
            return json.ToObject<JObject>(serializeNonPublic, loopSerialize, useCamelCase);
        }

        public static IEnumerable<dynamic> ToDynamicObjects(this string json,
                                                            bool serializeNonPublic = false,
                                                            bool loopSerialize = false,
                                                            bool useCamelCase = false)
        {
            return json.ToObject<JArray>(serializeNonPublic, loopSerialize);
        }
    }
}
