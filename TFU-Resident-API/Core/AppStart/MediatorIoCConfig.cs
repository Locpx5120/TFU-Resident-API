using Core.Infrastructure;
using Core.Infrastructure.Dapper;
using Core.Mapper;
using fake_tool.Core.Dapper;

namespace Core.AppStart
{
    public class MediatorIoCConfig
    {
        public static void ConfigureService(IServiceCollection services)
        {
            //Unit of work
            services.AddTransient<IUnitOfWork, UnitOfWork>();

            //Auto mapper
            services.AddAutoMapper(typeof(MappingProfile));

            //Repository
            //Dapper
            services.AddScoped<DapperCommon>();

            //Entity
            services.AddScoped<IUserIdentity, UserIdentity>();

            //Singleton
            services.AddSingleton<DapperDbContext>();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        }
    }
}
