using Misaka.Config;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Misaka.Message
{
    public class MessageHandlerProvider
    {
        private static readonly Dictionary<Type, Type[]> MessageHandlerTypes = new Dictionary<Type, Type[]>();

        public Type[] LookupHandlerTypes(Type messageType)
        {
            if (!MessageHandlerTypes.ContainsKey(messageType))
            {
                LoadMessageHandlers(messageType);
            }

            return MessageHandlerTypes[messageType];
        }

        private void LoadMessageHandlers(Type messageType)
        {
            var types = TypeProvider.Instance.FindTypeInfos(info =>
                                                            {
                                                                return info.GetInterfaces()
                                                                           .Any(i => i.IsGenericType
                                                                                  && i.GetGenericTypeDefinition() ==
                                                                                     typeof(IAsyncMessageSubscriber<>)
                                                                                  && i.GetGenericArguments()[0] == messageType);
                                                            });
            var handlers = new List<Type>();
            handlers.AddRange(types);
            MessageHandlerTypes[messageType] = handlers.ToArray();
        }
    }
}
