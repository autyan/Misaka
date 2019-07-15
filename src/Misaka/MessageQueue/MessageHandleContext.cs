using System;
using System.Collections.Generic;

namespace Misaka.MessageQueue
{
    public class MessageHandleContext
    {
        public MessageHandleContext(string topic,
                                    object message)
        {
            Topic       = topic;
            Message     = message;
            ConsumeTime = DateTime.Now;
        }

        public string Topic { get; }

        public object Message { get; }

        public DateTime ConsumeTime { get; }

        public IReadOnlyCollection<MessageHandleResult> HandleResults { get; private set; }

        public void SetHandleResult(MessageHandleResult[] results)
        {
            HandleResults = Array.AsReadOnly(results);
        }
    }

    public class MessageHandleResult
    {
        public MessageHandleResult(Type      messageHandler,
                                   Exception processError = null)
        {
            ProcessError   = processError;
            MessageHandler = messageHandler;

        }

        public Exception ProcessError { get; set; }

        public Type MessageHandler { get; set; }

        public bool HasError => ProcessError == null;
    }
}
