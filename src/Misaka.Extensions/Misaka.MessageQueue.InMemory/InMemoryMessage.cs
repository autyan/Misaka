namespace Misaka.MessageQueue.InMemory
{
    public class InMemoryMessage
    {
        public string Topic { get; set; }

        public object Message { get; set; }
    }
}
