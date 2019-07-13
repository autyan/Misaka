using Microsoft.EntityFrameworkCore;
using Misaka.MessageQueue;
using Misaka.MessageStore;
using System.Threading.Tasks;

namespace Misaka.EntityFrameworkCore
{
    public class EfMessageStore : DbContext, IMessageStore
    {
        public EfMessageStore(DbContextOptions options) : base(options)
        {
        }

        public DbSet<MessagePublished> MessagePublished { get; set; }

        public DbSet<MessageConsumed> MessageConsumed { get; set; }

        public DbSet<MessageHandle> MessageHandles { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<MessagePublished>().ToTable("_msg_published");
            modelBuilder.Entity<MessageConsumed>().ToTable("_msg_consumed");
            modelBuilder.Entity<MessageHandle>()
                        .ToTable("_msg_handles")
                        .HasOne<MessageConsumed>()
                        .WithMany()
                        .HasForeignKey(h => h.ConsumeId);
        }

        public async Task SavePublishAsync(PublishContext context)
        {
            MessagePublished.Add(new MessagePublished(context.Producer, 
                                                      context.Topic, 
                                                      context.Message.ToString(),
                                                      context.PublishTime, 
                                                      context.PublishError?.ToString()));
            await SaveChangesAsync();
        }

        public async Task SaveConsumeAsync(MessageHandleContext context)
        {
            var consumedMessage = new MessageConsumed(context.Topic,
                                                      context.Message.ToString(),
                                                      context.ConsumeTime);
            MessageConsumed.Add(consumedMessage);
            foreach (var result in context.HandleResults)
            {
                MessageHandles.Add(new MessageHandle(result.MessageHandler.Name,
                                                     result.ProcessError?.ToString(),
                                                     consumedMessage.Id));
            }

            await SaveChangesAsync();
        }
    }
}
