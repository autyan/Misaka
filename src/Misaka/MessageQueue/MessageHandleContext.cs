using System;
using System.Linq;

namespace Misaka.MessageQueue
{
    public class MessageHandleContext
    {
        public string Topic { get; set; }

        public object Message { get; set; }

        public Type MessageType { get; set; }

        public bool HasProcessed => HandleResults.All(r => !r.HasError);

        public MessageHandleResult[] HandleResults { get; set; }
    }

    public class MessageHandleResult
    {
        public Exception ProcessError { get; set; }

        public Type MessageHandler { get; set; }

        public bool HasError => ProcessError == null;
    }
}
