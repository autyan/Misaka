namespace Misaka.MessageQueue.Kafka
{
    internal class KafkaMessage
    {
        public string MessageType { get; set; }

        public string MessageBody { get; set; }

        public string MessageKey { get; set; }
    }
}
