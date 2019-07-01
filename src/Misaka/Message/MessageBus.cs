using Misaka.Message.Topics;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Misaka.Message
{
    public class MessageBus
    {
        private readonly IEnumerable<IMessageClient> _messageClients;

        private static readonly Dictionary<Type, string> MessageTopics;

        static MessageBus()
        {
            MessageTopics = new Dictionary<Type, string>();
        }

        public MessageBus(IEnumerable<IMessageClient> messageClients)
        {
            _messageClients = messageClients;
        }

        public void Publish(object message)
        {
            var topic = RetrieveMessageTopic(message);
            foreach (var client in _messageClients)
            {
                client.Publish(topic, message);
            }
        }

        public async Task PublishAsync(object message)
        {
            var topic = RetrieveMessageTopic(message);
            foreach (var client in _messageClients)
            {
                await client.PublishAsync(topic, message);
            }
        }

        public static string RetrieveMessageTopic(object message)
        {
            var messageType = message.GetType();
            if (!MessageTopics.ContainsKey(messageType))
            {
                LoadMessageTopic(messageType);
            }

            return MessageTopics[messageType];
        }

        private static void LoadMessageTopic(Type messageType)
        {
            var attrs = messageType.GetCustomAttributes(typeof(SendTopic), true);
            if (attrs.Length <= 0)
            {
                attrs = messageType.GetCustomAttributes(typeof(SendTopic), true);
            }

            var topic = attrs.Length > 0
                            ? ((Topic)attrs[0]).TopicName
                            : string.Empty;
            MessageTopics[messageType] = topic;
        }


    }
}
