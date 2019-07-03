using Misaka.Message;

namespace Misaka.Sample.Web
{
    [Topic("Test")]
    public class ValueEvent
    {
        public int Id { get; set; }

        public string Value { get; set; }
    }
}
