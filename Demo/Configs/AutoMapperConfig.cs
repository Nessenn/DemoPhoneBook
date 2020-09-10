using AutoMapper;
using Demo.AutoMapper;
using Microsoft.Extensions.DependencyInjection;

namespace Demo.Configs
{
    public static class AutoMapperConfig
    {
        public static void CreateConfig(IServiceCollection services)
        {
            var mapperConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new PhoneBookProfile());
            });

            var mapper = mapperConfig.CreateMapper();

            services.AddSingleton(mapper);
        }
    }
}
