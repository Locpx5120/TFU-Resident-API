using Core.Infrastructure;
using Core.Infrastructure.Dapper;
using Core.Mapper;
using fake_tool.Core.Dapper;
using Service;
using Service.Impl;
using TFU_Resident_API.Services;
using TFU_Resident_API.Services.Impl;

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

            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IInvestorService, InvestorService>();
            services.AddScoped<IProjectService, ProjectService>();
            services.AddScoped<IUserService, UserService>();
            //services.AddScoped<IBuildingService, BuildingService>();

            //Singleton
            services.AddSingleton<DapperDbContext>();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        }
    }
}
