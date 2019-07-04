using System.Threading.Tasks;

namespace Misaka.UnitOfWork
{
    public interface IUnitOfWork
    {
        void Commit();

        Task CommitAsync();
    }
}
