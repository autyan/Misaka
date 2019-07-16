using System;
using System.Collections.Generic;

namespace Misaka.MessageQueue.Kafka
{
    public class MessageTypeMatcher
    {
        private static readonly Dictionary<string, Type> RegisteredMessageType = new Dictionary<string, Type>();

        public static void Register(string source, Type target)
        {
            RegisteredMessageType.Add(source, target);
        }

        public static Type Lookup(string source)
        {
            if (RegisteredMessageType.ContainsKey(source))
            {
                return RegisteredMessageType[source];
            }

            foreach (var a in AppDomain.CurrentDomain.GetAssemblies())
            {
                var type = a.GetType(source);
                if (type != null)
                    return type;
            }

            return null;
        }
    }
}
