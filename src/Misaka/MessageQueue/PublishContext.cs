namespace Misaka.MessageQueue
{
    public class PublishContext
    {
        public string Topic { get; set; }

        public object Message { get; set; }
    }
}
