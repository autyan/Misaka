using Misaka.Utility;

namespace Misaka.EntityFrameworkCore
{
    public class MessageHandle
    {
        public MessageHandle(string handleName, 
                             string exception,
                             string consumeId)
        {
            Id         = IdentityUtility.NewGuidString();
            HandleName = handleName;
            Exception  = exception;
            ConsumeId  = consumeId;
        }

        public string Id { get; set; }

        public string HandleName { get; set; }

        public string Exception { get; set; }

        public string ConsumeId { get; set; }
    }
}
