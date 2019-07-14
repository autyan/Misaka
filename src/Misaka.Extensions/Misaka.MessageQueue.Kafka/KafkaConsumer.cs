using Microsoft.Extensions.Options;
using Misaka.DependencyInjection;
using Misaka.Message;
using Misaka.MessageStore;
using System;
using System.Threading;
using System.Threading.Tasks;
using Confluent.Kafka;
using Microsoft.Extensions.Logging;
using Misaka.Extensions.Json;

namespace Misaka.MessageQueue.Kafka
{
    public class KafkaConsumer : MessageConsumer
    {
        private readonly ILogger _logger;

        public KafkaConsumer(MessageHandlerProvider       provider,
                             IObjectProvider              objectProvider,
                             IOptionsMonitor<KafkaOption> option,
                             ILogger<KafkaConsumer>       logger,
                             IMessageStore                messageStore)
            : base(provider, objectProvider, option, messageStore)
        {
            _logger = logger;
        }

        public KafkaConsumer(MessageHandlerProvider       provider,
                             IObjectProvider              objectProvider,
                             IOptionsMonitor<KafkaOption> option,
                             ILogger<KafkaConsumer>       logger)
            : base(provider, objectProvider, option)
        {
            _logger = logger;
        }

        public override void Start()
        {
            DoStartAsync().Wait();
        }

        public override async Task StartAsync()
        {
            await DoStartAsync();
        }

        private Task DoStartAsync()
        {
            if (!(Option is KafkaOption option)) return Task.CompletedTask;

            foreach (var server in option.ConsumerServers)
            {
                    var consumeConfig = new ConsumerConfig()
                                        {
                                            GroupId          = option.GroupName,
                                            BootstrapServers = server,
                                            AutoOffsetReset  = AutoOffsetReset.Earliest
                                        };
                    Task.Factory.StartNew(() =>
                                          {
                                              using (var c = new ConsumerBuilder<Ignore, string>(consumeConfig).Build())
                                              {
                                                  foreach (var topic in option.Topics)
                                                  {
                                                      c.Subscribe(topic);
                                                  }

                                                  var cts = new CancellationTokenSource();

                                                  try
                                                  {
                                                      while (true)
                                                      {
                                                          try
                                                          {
                                                              var cr = c.Consume(cts.Token);
                                                              var @event = cr.Value.ToObject<KafkaMessage>();
                                                          }
                                                          catch (ConsumeException ex)
                                                          {
                                                              _logger.LogError(ex, "Kafka consume message error.");
                                                          }
                                                      }
                                                  }
                                                  catch (OperationCanceledException)
                                                  {
                                                      // Ensure the consumer leaves the group cleanly and final offsets are committed.
                                                      c.Close();
                                                  }
                                              }
                                          });

            }

            return Task.CompletedTask;
        }

        public override void Stop()
        {
            
        }
    }
}
