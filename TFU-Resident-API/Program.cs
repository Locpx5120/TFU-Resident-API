using BuildingModels;
using Core.AppStart;
using Core.Mapper;
using fake_tool.Helpers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using TFU_Resident_API.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin",
        builder => builder
            .AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader());
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("ServerCORS",
        builder => builder
            .WithOrigins("http://localhost:3000/")
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials());
});

builder.Services.AddControllers();

builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("SuperOwner"),
        b => b.MigrationsAssembly("TFU-Resident-API"));
});

builder.Services.AddDbContext<BuildingContext>(option =>
{
    option.UseSqlServer(builder.Configuration.GetConnectionString("BuildingTemplate"));
});

builder.Services.AddHttpContextAccessor();
builder.Services.AddControllers
(
     config =>
     {
         config.RespectBrowserAcceptHeader = true;
         config.ReturnHttpNotAcceptable = false;
     }
    )
    .AddJsonOptions
    (
    options =>
    {
        options.JsonSerializerOptions.Converters.Add(new DateOnlyJsonConverter());
    })
    .AddXmlDataContractSerializerFormatters();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddAutoMapper(typeof(MappingProfile));
MediatorIoCConfig.ConfigureService(builder.Services);
SwaggerConfig.ConfigureService(builder.Services, builder.Configuration);

//SignalR
builder.Services.AddSignalR(options =>
{
    options.EnableDetailedErrors = true;
    options.MaximumReceiveMessageSize = 102400000;
});

builder.Services.Configure<AppSettings>(builder.Configuration.GetSection("AppSettings"));

// configure jwt authentication
var secretKey = builder.Configuration["AppSettings:SecretKey"];
var key = Encoding.ASCII.GetBytes(secretKey);
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = false,
        ValidateAudience = false,

        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ClockSkew = TimeSpan.Zero,
    };
    options.Events = new JwtBearerEvents
    {
        OnMessageReceived = context =>
        {
            var accessToken = context.Request.Query["access_token"];

            var path = context.HttpContext.Request.Path;
            if (!string.IsNullOrEmpty(accessToken) &&
                (path.StartsWithSegments("/socket_web_client")))
            {
                context.Token = accessToken;
            }
            return System.Threading.Tasks.Task.CompletedTask;
        }
    };
});
builder.Services.AddAuthorization();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    SwaggerConfig.Configure(app, builder.Configuration, builder.Environment);
}

app.UseRouting();
app.UseCors("AllowSpecificOrigin");
app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.UseEndpoints(endpoints =>
{
});

app.Run();

