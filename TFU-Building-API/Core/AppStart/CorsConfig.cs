namespace Tun_Decor.Api.AppStart
{
    public class CorsConfig
    {
        private static readonly string CorePolicy = "CorePolicy";

        public static void ConfigureService(IServiceCollection services, IConfiguration conf)
        {
            services.AddCors(options =>
            {
                var origins = conf["Cors:AllowedOrigins"]?.Split("; ");
                options.AddPolicy(CorePolicy,
                    builder => builder
                    //.WithOrigins(origins)
                    .AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader());
                //.AllowCredentials());
            });
        }

        public static void Configure(IApplicationBuilder app, IConfiguration conf)
        {
            app.UseCors(CorePolicy);
        }
    }
}
