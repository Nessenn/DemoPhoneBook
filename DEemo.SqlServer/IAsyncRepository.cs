using System.Threading;
using System.Threading.Tasks;

namespace Demo.SqlServer
{
    public interface IAsyncRepository : IRepository
    {
        Task<T> GetAsync<T>(params object[] keyValues) where T : class;
        Task<T> GetAsync<T>(CancellationToken cancellationToken, params object[] keyValues) where T : class;
    }
}
