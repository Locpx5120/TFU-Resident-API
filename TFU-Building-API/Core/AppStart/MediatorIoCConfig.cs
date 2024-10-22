using TFU_Building_API.Core.Infrastructure;
using TFU_Building_API.Core.Infrastructure.Dapper;
using TFU_Building_API.Core.Mapper;
using TFU_Building_API.Service;
using TFU_Building_API.Service.impl;
using TFU_Building_API.Service.Impl;

namespace TFU_Building_API.Core.AppStart
{
    public class MediatorIoCConfig
    {
        public static void ConfigureService(IServiceCollection services)
        {
            //Unit of work
            services.AddTransient<IUnitOfWork, UnitOfWork>();

            //Auto mapper
            services.AddAutoMapper(typeof(MappingProfile));

            //Entity
            services.AddScoped<IAuthService, AuthService>();

            services.AddScoped<IUserIdentity, UserIdentity>();

            services.AddScoped<IStaffService, StaffService>();

            services.AddScoped<IRoleService, RoleService>();

            services.AddScoped<IBuildingService, BuildingService>();

            services.AddScoped<IOwnerShipService, OwnerShipService>();

            services.AddScoped<IResidentService, ResidentService>();


            //Singleton
            services.AddSingleton<DapperDbContext>();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

        }
    }
}
