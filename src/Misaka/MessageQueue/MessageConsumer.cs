using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Misaka.DependencyInjection;
using Misaka.Message;
using Misaka.MessageStore;

namespace Misaka.MessageQueue
{
    public abstract class MessageConsumer : IConsumer
    {
        protected MessageHandlerProvider Provider { get; }

        protected IObjectProvider ObjectProvider { get; }

        protected string[] Topics { get; }

        protected ConsumerOption Option { get; }

        protected IMessageStore MessageStore { get; }

        protected MessageConsumer(MessageHandlerProvider          provider,
                                  IObjectProvider                 objectProvider,
                                  IOptionsMonitor<ConsumerOption> option,
                                  IMessageStore                   messageStore)
        :this(provider, objectProvider, option)
        {
            MessageStore = messageStore;
        }

        protected MessageConsumer(MessageHandlerProvider          provider,
                                  IObjectProvider                 objectProvider,
                                  IOptionsMonitor<ConsumerOption> option)
        {
            Provider       = provider;
            ObjectProvider = objectProvider;
            Option = option.CurrentValue;

            Topics = Option.Topics ?? new string [0];
        }

        protected virtual async Task HandleMessageAsync(Func<MessageHandleContext> contextFunc)
        {
            var handleContext = contextFunc.Invoke();

            await ProcessAsync(handleContext);
        }

        protected virtual async Task ProcessAsync(MessageHandleContext handleContext)
        {
            if (handleContext.Message is string)
            {
                await PostProcessAsync(handleContext);
                return;
            }

            var messageType = handleContext.Message.GetType();
            var handlerTypes = Provider.LookupHandlerTypes(messageType);

            using (ObjectProvider.CreateScope())
            {
                var results = new MessageHandleResult[handlerTypes.Length];
                var index = 0;
                foreach (var handlerType in handlerTypes)
                {
                    using (ObjectProvider.CreateScope())
                    {
                        Exception error = null;
                        var method   = handlerType.GetMethod("HandleAsync", new[] { messageType });
                        var instance = ObjectProvider.GetService(handlerType);
                        try
                        {
                            var result = (Task)method?.Invoke(instance, new[] { handleContext.Message });
                            if (result != null)
                            {
                                await result;
                            }
                        }
                        catch (Exception ex)
                        {
                            error = ex;
                        }

                        var handleResult = new MessageHandleResult(handlerType, error);
                        results[index] = handleResult;
                        index++;
                    }
                }

                handleContext.SetHandleResult(results);
            }

            await PostProcessAsync(handleContext);
        }

        protected virtual async Task PostProcessAsync(MessageHandleContext context)
        {
            if (MessageStore != null)
            {
                await MessageStore.SaveConsumeAsync(context);
            }
        }

        public abstract void Start();

        public abstract Task StartAsync();

        public abstract void Stop();
    }
}
