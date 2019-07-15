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

        private async Task DoStartAsync()
        {
            if (!(Option is KafkaOption option))
            {
                _logger.LogCritical("kafka option not found");
                return;
            }

            foreach (var server in option.ConsumerServers)
            {
                    var consumeConfig = new ConsumerConfig
                                        {
                                            GroupId          = option.GroupName,
                                            BootstrapServers = server,
                                            AutoOffsetReset  = AutoOffsetReset.Earliest
                                        };
                await Task.Factory.StartNew(async () =>
                {
                    while (true)
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
                                        var messageType =
                                            MessageTypeMatcher.Lookup(@event.MessageType);

                                        object messageObj;
                                        if (messageType == null)
                                        {
                                            _logger.LogError("message type not find");
                                            messageObj = @event.MessageType;
                                        }
                                        else
                                        {
                                            messageObj = @event.MessageType.ToObject(messageType);
                                        }

                                        await HandleMessageAsync(() => new MessageHandleContext(cr.Topic, messageObj));

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
                            catch (Exception ex)
                            {
                                _logger.LogError(ex, "consume error");
                            }
                        }
                    }
                    // ReSharper disable once FunctionNeverReturns
                });

            }
        }

        public override void Stop()
        {
            
        }
    }
}
