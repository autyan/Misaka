using System;
using Misaka.Domain;
using Misaka.Utility;

namespace Misaka.MessageQueue
{
    public sealed class PublishContext
    {
        public PublishContext(string topic, 
                              object message, 
                              string producer)
        {
            Topic = topic;
            Message = message;
            PublishTime = DateTime.Now;
            Producer = producer;
            if (Message is AggregateRootEvent @event)
            {
                Key = @event.Key;
            }
            else
            {
                Key = IdentityUtility.NewGuidString();
            }
        }

        public string Topic { get; }

        public object Message { get; }

        public DateTime PublishTime { get; }

        public string Producer { get; }

        public Exception PublishError { get; private set; }

        public string Key { get; }

        public void SetError(Exception error)
        {
            PublishError = error;
        }
    }
}
