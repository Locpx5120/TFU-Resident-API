using Microsoft.OpenApi.Models;

namespace Core.AppStart
{
    public class SwaggerConfig
    {
        public static void ConfigureService(IServiceCollection services, IConfiguration configuration)
        {
            services.AddSwaggerGen(swagger =>
            {
                swagger.SwaggerDoc("v2.2.1", new OpenApiInfo
                {
                    Version = "v2.2.1",
                    Title = "Resident API",
                    Description = "Resident API ver1.0"
                });

                // To Enable authorization using Swagger (JWT)  
                swagger.AddSecurityDefinition("basic", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    Scheme = "basic",
                    In = ParameterLocation.Header,
                    Description = "Basic Authorization header using the Bearer scheme."
                });

                swagger.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description =
                         "Enter 'Bearer' [space] and then your valid token in the text input below.\r\n\r\nExample: \"Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9\"",
                });

                swagger.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        new string[] {}
                    }
                });

                //var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                //swagger.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
                swagger.ResolveConflictingActions(x => x.First());
            });
        }


        public static void Configure(IApplicationBuilder app, IConfiguration configuration, IWebHostEnvironment env)
        {

            if (env.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(swagger =>
                {
                    swagger.SwaggerEndpoint("v2.2.1/swagger.json", "Resident API ver1.0");
                    swagger.InjectJavascript("https://ajax.googleapis.com/ajax/libs/jquery/3.3.1/jquery.min.js");
                    swagger.InjectJavascript("swagger-basic-auth.js");
                });
            }
        }
    }
}
