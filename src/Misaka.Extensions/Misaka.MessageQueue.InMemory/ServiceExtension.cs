using Misaka.Config;

namespace Misaka.MessageQueue.InMemory
{
    public static class ServiceExtension
    {
        public static Config.Configuration UseInMemoryQueue(this Configuration configuration)
        {
            return configuration;
        }
    }
}
