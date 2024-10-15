using AutoMapper;
using BuildingModels;
using Constant;
using Core.Enums;
using Core.Handler;
using Core.Infrastructure;
using Core.Model;
using fake_tool.Helpers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Options;
using TFU_Resident_API.Core.Helper;
using TFU_Resident_API.Data;
using TFU_Resident_API.Dto;

namespace TFU_Resident_API.Services.Impl
{
    public class BuildingService : BaseHandler, IBuildingService
    {
        private readonly IConfiguration _config;
        private readonly AppSettings _appSettings;
        private readonly IUserIdentity _userIdentity;
        private readonly IMapper _mapper;
        private readonly EmailService emailService;

        private readonly AppDbContext superOwnerContext;
        private readonly BuildingContext buildingContext;

        public BuildingService(
            IOptionsMonitor<AppSettings> optionsMonitor,
            IConfiguration config,
            IUnitOfWork unitOfWork,
            IHttpContextAccessor httpContextAccessor,
            IUserIdentity userIdentity,
            IMapper mapper,
            BuildingContext buildingContext
        ) : base(unitOfWork, httpContextAccessor)
        {
            _appSettings = optionsMonitor.CurrentValue;
            _config = config;
            _userIdentity = userIdentity;
            _mapper = mapper;
            emailService = new EmailService(_config);
            this.buildingContext = buildingContext;
        }

        public async Task<ResponseData<object>> Create(CreateBuildingDto createBuildingDto)
        {
            using (IDbContextTransaction transaction = superOwnerContext.Database.BeginTransaction())
            {
                SuperOwnerModels.Building building = this._mapper.Map<SuperOwnerModels.Building>(createBuildingDto);
                try
                {
                    transaction.CreateSavepoint("before_create_building");

                    var project = this.superOwnerContext.Projects.FirstOrDefault(x => x.Id == createBuildingDto.ProjectId);
                    string connectionTemplate = _config.GetConnectionString("BuildingTemplate");
                    var investorDbServerConfigs = _config.GetSection("MultiTenantDbSetting");
                    string connection = connectionTemplate;

                    building.ConnectionString = connection;

                    UnitOfWork.BuildingRepository.Add(building);
                    await UnitOfWork.SaveChangesAsync();

                    //var newBuilding = this.superOwnerContext.Add(building);
                    //this.superOwnerContext.SaveChanges();

                    connection = string.Format(connectionTemplate,
                        investorDbServerConfigs["Server"],
                        $"a_{project.InvestorId}_{project.Id}_{building.Id}".Replace("-", "_"),
                        investorDbServerConfigs["User"],
                        investorDbServerConfigs["Password"]);

                    this.buildingContext.Database.SetConnectionString(connection);
                    this.buildingContext.Database.Migrate();
                    building.ConnectionString = connection;

                    UnitOfWork.BuildingRepository.Update(building);
                    await UnitOfWork.SaveChangesAsync();

                    //this.superOwnerContext.SaveChanges();

                    transaction.ReleaseSavepoint("before_create_building");
                    transaction.Commit();
                }
                catch (Exception e)
                {
                    transaction.RollbackToSavepoint("before_create_building");

                    return new ResponseData<object>()
                    {
                        Success = false,
                        Message = e.Message,
                        Code = (int)ErrorCodeAPI.SystemIsError,
                    };
                }

                return new ResponseData<object>()
                {
                    Success = true,
                    Message = MessConstant.Successfully,
                    Code = (int)ErrorCodeAPI.OK,
                };
            }
        }
    }
}
