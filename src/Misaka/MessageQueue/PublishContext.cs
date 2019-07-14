using System;

namespace Misaka.MessageQueue
{
    public sealed class PublishContext
    {
        public PublishContext()
        {
            PublishTime = DateTime.Now;
        }

        public string Topic { get; set; }

        public object Message { get; set; }

        public DateTime PublishTime { get; }

        public string Producer { get; set; }

        public Exception PublishError { get; set; }

        public string Key { get; set; }
    }
}
