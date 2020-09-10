using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Threading.Tasks;

namespace Demo.SqlServer
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        private DbContext _context = null;

        public UnitOfWork(IConfiguration configuration, bool isUseInMemory = false)
        {
            var optionsBuilder = new DbContextOptionsBuilder<DemoContext>();
            if (isUseInMemory)
            {
                optionsBuilder.UseInMemoryDatabase(databaseName: "DemoTestDB");
            }
            else
            {
                var connectionString = configuration.GetConnectionString("DefaultConnection");
                optionsBuilder.UseSqlServer(connectionString);
            }

            this._context = new DemoContext(optionsBuilder.Options);
        }

        private IRepository _myEntityRepository;

        public IRepository Repository
        {
            get
            {
                return this._myEntityRepository ?? (this._myEntityRepository = new DemoGenericRepository(this._context));
            }
        }

        public void SaveChanges()
        {
            this._context.SaveChanges();
        }

        public async Task SaveChangesAsync()
        {
           await this._context.SaveChangesAsync();
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (this._context != null)
                {
                    this._context.Dispose();
                    this._context = null;
                }
            }
        }
    }
}
