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

        protected virtual async Task OnMessageReceive(MessageHandleContext handleContext)
        {
            await BeforeMessageProcess(handleContext);

            var handlerTypes = Provider.LookupHandlerTypes(handleContext.MessageType);

            using (ObjectProvider.CreateScope())
            {
                foreach (var handlerType in handlerTypes)
                {
                    var method   = handlerType.GetMethod("HandleAsync", new[] { handleContext.Message.GetType() });
                    var instance = ObjectProvider.GetService(handlerType);
                    method?.Invoke(instance, new[] { handleContext.Message });
                }
            }

            await AfterMessageProcessed(handleContext);
        }

        protected virtual Task BeforeMessageProcess(MessageHandleContext handleContext)
        {
            return Task.CompletedTask;
        }

        protected virtual Task AfterMessageProcessed(MessageHandleContext handleContext)
        {
            return Task.CompletedTask;
        }

        public abstract void Start();

        public abstract Task StartAsync();
    }
}
