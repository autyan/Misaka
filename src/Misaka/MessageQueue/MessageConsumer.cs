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

        protected MessageConsumer(MessageHandlerProvider provider,
                                  IObjectProvider        objectProvider)
        {
            Provider       = provider;
            ObjectProvider = objectProvider;
        }

        protected virtual async Task HandleMessageAsync(Func<MessageHandleContext> contextFunc)
        {
            var handleContext = contextFunc.Invoke();

            await BeforeProcessAsync(handleContext)
               .ConfigureAwait(false);

            await ProcessAsync(handleContext)
               .ConfigureAwait(false);

            await AfterProcessedAsync(handleContext)
               .ConfigureAwait(false);
        }

        protected virtual async Task ProcessAsync(MessageHandleContext handleContext)
        {
            var handlerTypes = Provider.LookupHandlerTypes(handleContext.MessageType);

            using (ObjectProvider.CreateScope())
            {
                handleContext.HandleResults = new MessageHandleResult[handlerTypes.Length];
                var index = 0;
                foreach (var handlerType in handlerTypes)
                {
                    var handleResult = new MessageHandleResult
                                       {
                                           MessageHandler = handlerType
                                       };
                    var method   = handlerType.GetMethod("HandleAsync", new[] { handleContext.Message.GetType() });
                    var instance = ObjectProvider.GetService(handlerType);
                    try
                    {
                        var result = (Task) method?.Invoke(instance, new[] {handleContext.Message});
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
