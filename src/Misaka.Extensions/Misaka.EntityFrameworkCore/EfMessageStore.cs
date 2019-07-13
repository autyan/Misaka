using Microsoft.EntityFrameworkCore;
using Misaka.MessageQueue;
using Misaka.MessageStore;
using System;
using System.Threading.Tasks;

namespace Misaka.EntityFrameworkCore
{
    public class EfMessageStore : DbContext, IMessageStore
    {
        public Task SavePublishAsync(PublishContext context)
        {
            throw new NotImplementedException();
        }

        public Task SaveConsumeAsync(MessageHandleContext contexts)
        {
            throw new NotImplementedException();
        }
    }
}
