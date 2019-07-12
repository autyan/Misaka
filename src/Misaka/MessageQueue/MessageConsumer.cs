using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Misaka.DependencyInjection;
using Misaka.Message;

namespace Misaka.MessageQueue
{
    public abstract class MessageConsumer : IConsumer
    {
        protected MessageHandlerProvider Provider { get; }

        protected IObjectProvider ObjectProvider { get; }

        protected string[] Topics { get; }

        protected ConsumerOption Option { get; }
        
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

        public abstract void Start();

        public abstract Task StartAsync();
        public abstract void Stop();
    }
}
