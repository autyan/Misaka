using Misaka.Extensions.Json;

namespace Misaka.MessageQueue.Kafka
{
    internal class KafkaMessage
    {
        public KafkaMessage()
        {

        }

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

        public string MessageType { get; protected set; }

        public string MessageBody { get; protected set; }

        public string MessageKey { get; protected set; }

        public string Host { get; protected set; }

        public string App { get; protected set; }
    }
}
