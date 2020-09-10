using Microsoft.Extensions.Configuration;

namespace Demo.SqlServer
{
    public class UnitOfWorkFactory : IUnitOfWorkFactory
    {
        private IConfiguration _configuration;
        private bool _isUseInMemory;
        public UnitOfWorkFactory(IConfiguration configuration, bool isUseInMemory = false)
        {
            this._configuration = configuration;
            this._isUseInMemory = isUseInMemory;
        }

        public UnitOfWork Create()
        {
            return new UnitOfWork(this._configuration, this._isUseInMemory);
        }
    }
}
