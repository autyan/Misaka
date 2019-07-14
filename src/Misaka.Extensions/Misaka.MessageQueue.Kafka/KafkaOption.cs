namespace Misaka.MessageQueue.Kafka
{
    public class KafkaOption : ConsumerOption
    {
        public string PublishServer { get; set; }

        public string[] ConsumerServers { get; set; }

        public string GroupName { get; set; }
    }
}
