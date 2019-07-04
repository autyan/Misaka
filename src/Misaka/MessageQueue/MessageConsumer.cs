using System;
using System.Threading.Tasks;
using Misaka.DependencyInjection;
using Misaka.Message;

namespace Misaka.MessageQueue
{
    public abstract class MessageConsumer : IConsumer
    {
        protected MessageHandlerProvider Provider { get; }

        protected IObjectProvider ObjectProvider { get; }

        protected string[] Topics { get; }

        protected MessageConsumer(MessageHandlerProvider provider,
                                  IObjectProvider        objectProvider,
                                  ConsumerOption         option = null)
        {
            Provider       = provider;
            ObjectProvider = objectProvider;
            if (option == null)
            {
                option = new ConsumerOption();
            }

            Topics = option.Topics ?? new string [0];
        }

        protected virtual async Task HandleMessageAsync(Func<MessageHandleContext> contextFunc)
        {
            var handleContext = contextFunc.Invoke();

            await BeforeProcessAsync(handleContext);

            await ProcessAsync(handleContext);

            await AfterProcessedAsync(handleContext);
        }

        protected virtual async Task ProcessAsync(MessageHandleContext handleContext)
        {
            var messageType = handleContext.Message.GetType();
            var handlerTypes = Provider.LookupHandlerTypes(messageType);

            using (ObjectProvider.CreateScope())
            {
                handleContext.HandleResults = new MessageHandleResult[handlerTypes.Length];
                var index = 0;
                foreach (var handlerType in handlerTypes)
                {
                    using (ObjectProvider.CreateScope())
                    {
                        var handleResult = new MessageHandleResult
                                           {
                                               MessageHandler = handlerType
                                           };
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
                            handleResult.ProcessError = ex;
                        }

                        handleContext.HandleResults[index] = handleResult;
                        index++;
                    }
                }
            }
        }

        protected virtual Task BeforeProcessAsync(MessageHandleContext handleContext)
        {
            return Task.CompletedTask;
        }

        protected virtual Task AfterProcessedAsync(MessageHandleContext handleContext)
        {
            return Task.CompletedTask;
        }

        public abstract void Start();

        public abstract Task StartAsync();
        public abstract void Stop();
    }
}
