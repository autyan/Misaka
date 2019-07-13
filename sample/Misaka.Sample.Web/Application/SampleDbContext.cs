using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Misaka.EntityFrameworkCore;

namespace Misaka.Sample.Web.Application
{
    public class SampleDbContext : EfMessageStore
    {
        public SampleDbContext(DbContextOptions options) : base(options)
        {

        }
    }
}
