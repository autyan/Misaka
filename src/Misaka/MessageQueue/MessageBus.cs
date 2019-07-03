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
           DoPublishAsync(message).GetAwaiter().GetResult();
        }

        public async Task PublishAsync(object message)
        {
            await DoPublishAsync(message).ConfigureAwait(false);
        }

        private async Task DoPublishAsync(object message)
        {
            var context = new PublishContext
                          {
                              Topic   = RetrieveTopic(message),
                              Message = message
                          };
            foreach (var producer in _producers)
            {
                try
                {
                    await producer.PublishAsync(context)
                                  .ConfigureAwait(false);
                }
                catch (Exception ex)
                {
                    context.PublishError = ex;
                }
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
