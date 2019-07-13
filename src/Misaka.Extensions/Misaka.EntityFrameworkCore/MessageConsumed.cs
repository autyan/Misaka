using System;
using Misaka.Utility;

namespace Misaka.EntityFrameworkCore
{
    public class MessageConsumed
    {
        public MessageConsumed(string   topic,
                               string   messageBody,
                               DateTime consumeTime)
        {
            Id          = IdentityUtility.NewGuidString();
            Topic       = topic;
            MessageBody = messageBody;
            ConsumeTime = consumeTime;
        }

        public string Id { get; protected set; }

        public string Topic { get; protected set; }

        public string MessageBody { get; protected set; }

        public DateTime ConsumeTime { get; protected set; }
    }
}
