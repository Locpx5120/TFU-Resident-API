using TFU_Building_API.Core.Mapper;

namespace TFU_Building_API.Core.AppStart
{
    public class MediatorIoCConfig
    {
        public static void ConfigureService(IServiceCollection services)
        {
            //Unit of work
            //services.AddTransient<IUnitOfWork, UnitOfWork>();

            //Auto mapper
            services.AddAutoMapper(typeof(MappingProfile));

        }
    }
}
