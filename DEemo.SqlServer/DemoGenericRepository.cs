using Demo.DbModel.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Demo.SqlServer
{
    public class DemoGenericRepository : IAsyncRepository
    {
        private readonly DbContext _context;

        public DemoGenericRepository(DbContext dbContext)
        {
            _context = dbContext;
        }

        protected DbContext Context
        {
            get { return _context; }
        }

        public IQueryable<T> Query<T>() where T : class
        {
            return _context.Set<T>();
        }

        public IQueryable<T> QueryNoTracking<T>() where T : class
        {
            return _context.Set<T>().AsNoTracking();
        }

        public T Get<T>(params object[] keyValues) where T : class
        {
            return Context.Set<T>().Find(keyValues);
        }

        public async Task<T> GetAsync<T>(params object[] keyValues) where T : class
        {
            return await _context.Set<T>().FindAsync(keyValues);
        }

        public async Task<T> GetAsync<T>(CancellationToken cancellationToken, params object[] keyValues) where T : class
        {
            return await _context.Set<T>().FindAsync(cancellationToken, keyValues);
        }

        public T Add<T>(T entity) where T : class
        {
            if (entity is IDateTimeEntity)
            {
                ((IDateTimeEntity)entity).Created = DateTime.UtcNow;
                ((IDateTimeEntity)entity).Updated = DateTime.UtcNow;
            }

            Context.Set<T>().Add(entity);
            return entity;
        }

        public T Update<T>(T entity) where T : class
        {

            if (entity is IDateTimeEntity)
            {
                ((IDateTimeEntity)entity).Updated = DateTime.UtcNow;
            }

            Context.Set<T>().Attach(entity);
            Context.Entry(entity).State = EntityState.Modified;
            return entity;
        }

        public void Remove<T>(params object[] keyValues) where T : class
        {
            var entity = Get<T>(keyValues);
            Context.Set<T>().Remove(entity);
        }
    }
}
