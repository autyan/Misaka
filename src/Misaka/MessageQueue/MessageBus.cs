using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Misaka.Message;

namespace Misaka.MessageQueue
{
    public class MessageBus : IMessageBus
    {
        private readonly IEnumerable<IProducer> _producers;

        private readonly Dictionary<Type, string> _messageTopics = new Dictionary<Type, string>();

        public MessageBus(IEnumerable<IProducer> producers)
        {
            _producers = producers;
        }

        public void Publish(object message)
        {
            foreach (var producer in _producers)
            {
                producer.Publish(new PublishContext
                                 {
                                     Topic   = RetrieveTopic(message),
                                     Message = message
                                 });
            }
        }

        public async Task PublishAsync(object message)
        {
            foreach (var producer in _producers)
            {
                await producer.PublishAsync(new PublishContext
                                            {
                                                Topic   = RetrieveTopic(message),
                                                Message = message
                                            })
                              .ConfigureAwait(false);
            }
        }

        private string RetrieveTopic(object message)
        {
            var messageType = message.GetType();
            if (!_messageTopics.ContainsKey(messageType))
            {
                LoadMessageTopic(messageType);
            }
            return _messageTopics[messageType];
        }

        private void LoadMessageTopic(Type messageType)
        {
            var publishTopic = messageType.GetCustomAttributes(typeof(PublishTopic), false);
            if (publishTopic.Length > 0)
            {
                _messageTopics[messageType] = ((PublishTopic) publishTopic[0]).TopicName;
                return;
            }

            var topic = messageType.GetCustomAttributes(typeof(Topic), false);
            if (topic.Length > 0)
            {
                _messageTopics[messageType] = ((Topic)topic[0]).TopicName;
                return;
            }

            _messageTopics[messageType] = string.Empty;
        }
    }
}
