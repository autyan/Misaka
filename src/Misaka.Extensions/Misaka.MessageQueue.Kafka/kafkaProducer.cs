using System;
using Confluent.Kafka;
using Microsoft.Extensions.Options;
using Misaka.Extensions.Json;
using System.Threading.Tasks;
using Misaka.Utility;

namespace Misaka.MessageQueue.Kafka
{
    public class KafkaProducer : IProducer
    {
        private readonly IProducer<string, string> _kafkaProducer;

        private readonly string _host;

        private readonly string _app;

        private KafkaProducer()
        {
            _host = ApplicationUtility.GetHostIpAddress();
            _app  = ApplicationUtility.GetCurrentApplicationName();
        }

        public KafkaProducer(IOptionsMonitor<KafkaOption> option) : this()
        {
            var option1 = option.CurrentValue;
            _kafkaProducer = new ProducerBuilder<string, string>(new ProducerConfig
                                                                 {
                                                                     BootstrapServers = option1.PublishServer,
                                                                     //Partitioner = Partitioner.Murmur2Random
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
            var dr = await _kafkaProducer.ProduceAsync(context.Topic, new Message<string, string>
                                                             {
                                                                 Key = context.Key,
                                                                 Value = new KafkaMessage(context.Message, 
                                                                                          context.Key,
                                                                                          _host,
                                                                                          _app).ToJson()
                                                             });
            Console.WriteLine(dr);
        }

        public string Name { get; } = nameof(KafkaProducer);

        public void Dispose()
        {
            _kafkaProducer.Dispose();
        }
    }
}
