using Demo.Infrastructure.Interafaces;
using Demo.Infrastructure.Services;
using Demo.SqlServer;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace Demo.Configs
{
    public static class DIConfig
    {
        public static void Config(IServiceCollection services)
        {
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddTransient<IPhoneBookService, PhoneBookService>();
            services.AddTransient<IUnitOfWorkFactory, UnitOfWorkFactory>();
        }
    }
}
