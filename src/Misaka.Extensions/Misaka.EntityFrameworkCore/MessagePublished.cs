using System;
using Misaka.Utility;

namespace Misaka.EntityFrameworkCore
{
    public class MessagePublished
    {
        public MessagePublished(string   producer,
                                string   topic,
                                string   messageBody,
                                DateTime publishTime,
                                string   publishError)
        {
            Id           = IdentityUtility.NewGuidString();
            Producer     = producer;
            Topic        = topic;
            MessageBody  = messageBody;
            PublishTime  = publishTime;
            PublishError = publishError;
        }

        public string Id { get; protected set; }

        public string Producer { get; protected set; }

        public string Topic { get; protected set; }

        public string MessageBody { get; protected set; }

        public DateTime PublishTime { get; protected set; }

        public string PublishError { get; protected set; }
    }
}
