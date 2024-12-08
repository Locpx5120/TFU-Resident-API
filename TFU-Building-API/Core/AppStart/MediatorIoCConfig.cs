using TFU_Building_API.Core.Dapper;
using TFU_Building_API.Core.Dapper.ServiceContract;
using TFU_Building_API.Core.Dapper.User;
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

            services.AddScoped<IServiceApartment, ServiceApartment>();

            services.AddScoped<IInvoiceService, InvoiceService>();

            services.AddScoped<IServiceContractService, ServiceContractService>();

            services.AddScoped<IService, ServiceImplement>();

            services.AddScoped<IPackageService, PackageService>();

            services.AddScoped<IResidentPayment, ResidentPaymentService>();

            services.AddScoped<IApartmentService, ApartmentService>();
            services.AddScoped<IPaymentHistoryService, PaymentHistoryService>();

            services.AddScoped<IServiceCategoryService, ServiceCategoryService>();
            services.AddScoped<IServiceRequestService, ServiceRequestService>();
            services.AddScoped<IApartmentType, ApartmentTypeService>();

            services.AddScoped<IThirdPartyService, ThirdPartyService>();
            services.AddScoped<INotifyService, NotifyService>();
            services.AddScoped<INotifyCategoryService, NotifyCategoryService>();

            //dapter
            services.AddTransient<DapperCommon>();
            services.AddSingleton<DapperDbContext>();
            services.AddScoped<IServiceContractRepository, ServiceContractRepository>();
            services.AddScoped<IUserRepository, UserRepository>();

            //Singleton
            //services.AddScoped<IDapperDbContext, DapperDbContext>();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

        }
    }
}
