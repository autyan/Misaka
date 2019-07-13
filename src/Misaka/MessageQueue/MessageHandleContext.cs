using System;

namespace Misaka.MessageQueue
{
    public class MessageHandleContext
    {
        public MessageHandleContext()
        {
            ConsumeTime = DateTime.Now;
        }

        public string Topic { get; set; }

        public object Message { get; set; }

        public DateTime ConsumeTime { get; }

        public MessageHandleResult[] HandleResults { get; set; }
    }

    public class MessageHandleResult
    {
        public Exception ProcessError { get; set; }

        public Type MessageHandler { get; set; }

        public bool HasError => ProcessError == null;
    }
}
