using System;

namespace Misaka.Message
{
    [AttributeUsage(AttributeTargets.Class)]
    public class Topic : Attribute
    {
        public Topic(string topicName)
        {
            TopicName = topicName;
        }

        public string TopicName { get; }
    }
}
