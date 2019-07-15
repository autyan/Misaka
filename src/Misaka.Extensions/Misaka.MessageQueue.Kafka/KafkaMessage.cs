using Misaka.Extensions.Json;

namespace Misaka.MessageQueue.Kafka
{
    internal class KafkaMessage
    {
        public KafkaMessage(object message, 
                            string messageKey, 
                            string host,
                            string app)
        {
            MessageType = message.GetType().FullName;
            MessageBody = message.ToJson();
            MessageKey  = messageKey;
            Host        = host;
            App         = app;
        }

        public string MessageType { get; }

        public string MessageBody { get; }

        public string MessageKey { get; }

        public string Host { get; }

        public string App { get; set; }
    }
}
