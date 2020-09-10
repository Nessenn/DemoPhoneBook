using System.Threading.Tasks;

namespace Demo.SqlServer
{
    public interface IUnitOfWork
    {
        IRepository Repository { get; }

        void SaveChanges();
        Task SaveChangesAsync();
    }
}
