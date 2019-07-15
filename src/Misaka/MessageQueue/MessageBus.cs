using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Misaka.Message;
using Misaka.MessageStore;

namespace Misaka.MessageQueue
{
    public class MessageBus : IMessageBus
    {

        private readonly IEnumerable<IProducer> _producers;

        private readonly Dictionary<Type, string> _messageTopics = new Dictionary<Type, string>();

        private readonly IMessageStore _messageStore;

        public MessageBus(IEnumerable<IProducer> produces,
                          IMessageStore          messageStore)
            : this(produces)
        {
            _messageStore = messageStore;
        }

        public MessageBus(IEnumerable<IProducer> producers)
        {
            _producers = producers;
        }

        public void Publish(object message)
        {
           DoPublishAsync(message).Wait();
        }

        public void Publish(IEnumerable messages)
        {
            foreach (var message in messages)
            {
                DoPublishAsync(message).Wait();
            }
        }

        public async Task PublishAsync(object message)
        {
            await DoPublishAsync(message);
        }

        public async Task PublishAsync(IEnumerable messages)
        {
            foreach (var message in messages)
            {
                await DoPublishAsync(message);
            }
        }

        private async Task DoPublishAsync(object message)
        {
            var topic = RetrieveTopic(message);
            foreach (var producer in _producers)
            {
                var context = new PublishContext(topic, message, producer.Name);
                try
                {
                    await producer.PublishAsync(context);
                }
                catch (Exception ex)
                {
                    context.SetError(ex);
                }

                if (_messageStore != null)
                {
                    await _messageStore.SavePublishAsync(context);
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

        public void Dispose()
        {
            
        }
    }
}
