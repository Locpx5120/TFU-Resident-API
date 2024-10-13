using AutoMapper;
using BuildingModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using TFU_Resident_API.Data;
using TFU_Resident_API.Dto;

namespace TFU_Resident_API.Controllers
{
    [Route("api/building")]
    [ApiController]
    public class BuildingController : ControllerBase
    {
        IConfiguration configuration;
        readonly AppDbContext superOwnerContext;
        readonly IMapper mapper;
        readonly BuildingContext buildingContext;

        public BuildingController(AppDbContext superOwnerContext, IMapper mapper, IConfiguration configuration, BuildingContext buildingContext)
        {
            this.superOwnerContext = superOwnerContext;
            this.mapper = mapper;
            this.configuration = configuration;
            this.buildingContext = buildingContext;
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateBuildingDto createBuildingDto)
        {
            using (IDbContextTransaction transaction = superOwnerContext.Database.BeginTransaction())
            {
                SuperOwnerModels.Building building = this.mapper.Map<SuperOwnerModels.Building>(createBuildingDto);
                try
                {
                    transaction.CreateSavepoint("before_create_building");
                    var project = this.superOwnerContext.Projects.FirstOrDefault(x => x.Id == createBuildingDto.ProjectId);
                    string connectionTemplate = configuration.GetConnectionString("BuildingTemplate");
                    var investorDbServerConfigs = configuration.GetSection("MultiTenantDbSetting");
                    string connection = connectionTemplate;

                    building.ConnectionString = connection;
                    var newBuilding = this.superOwnerContext.Add(building);
                    this.superOwnerContext.SaveChanges();

                    connection = string.Format(connectionTemplate,
                        investorDbServerConfigs["Server"],
                        $"a_d".Replace("-", "_"),
                        investorDbServerConfigs["User"],
                        investorDbServerConfigs["Password"]);

                    this.buildingContext.Database.SetConnectionString(connection);
                    this.buildingContext.Database.Migrate();
                    building.ConnectionString = connection;
                    this.superOwnerContext.Update(building);
                    this.superOwnerContext.SaveChanges();

                    transaction.ReleaseSavepoint("before_create_building");
                    transaction.Commit();
                }
                catch (Exception)
                {
                    transaction.RollbackToSavepoint("before_create_building");
                    throw;
                }
                return Ok(building);
            }
        }
    }
}
