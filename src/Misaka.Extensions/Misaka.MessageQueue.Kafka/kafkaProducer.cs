using Confluent.Kafka;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;

namespace Misaka.MessageQueue.Kafka
{
    public class KafkaProducer : IProducer
    {
        private readonly IProducer<string, string> _kafkaProducer;

        public KafkaProducer(IOptionsMonitor<KafkaOption> option)
        {
            var option1 = option.CurrentValue;
            _kafkaProducer = new ProducerBuilder<string, string>(new ProducerConfig
                                                                 {
                                                                     BootstrapServers = option1.PublishServer,
                                                                     Partitioner = Partitioner.Murmur2Random
                                                                 })
               .Build();
        }

        public void Publish(PublishContext context)
        {
            DoPublishAsync(context).Wait();
        }

        public async Task PublishAsync(PublishContext context)
        {
            await DoPublishAsync(context);
        }

        private async Task DoPublishAsync(PublishContext context)
        {
            await _kafkaProducer.ProduceAsync(context.Topic, new Message<string, string>
                                                             {
                                                                 Key = context.Key,
                                                                 Value = context.Message.ToString()
                                                             });
        }

        public string Name { get; } = nameof(KafkaProducer);

        public void Dispose()
        {
        }
    }
}
