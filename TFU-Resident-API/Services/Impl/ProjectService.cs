﻿using AutoMapper;
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
using SuperOwnerModels;
using TFU_Resident_API.Core.Helper;
using TFU_Resident_API.Data;
using TFU_Resident_API.Dto;

namespace TFU_Resident_API.Services.Impl
{
    public class ProjectService : BaseHandler, IProjectService
    {
        private readonly IConfiguration _config;
        private readonly AppSettings _appSettings;
        private readonly IUserIdentity _userIdentity;
        private readonly IMapper _mapper;
        private readonly EmailService emailService;
        private readonly AppDbContext superOwnerContext;
        private readonly BuildingContext buildingContext;

        public ProjectService(
            IOptionsMonitor<AppSettings> optionsMonitor,
            IConfiguration config,
            IUnitOfWork unitOfWork,
            IHttpContextAccessor httpContextAccessor,
            IUserIdentity userIdentity,
            IMapper mapper,
            BuildingContext buildingContext,
            AppDbContext superOwnerContext
        ) : base(unitOfWork, httpContextAccessor)
        {
            _appSettings = optionsMonitor.CurrentValue;
            _config = config;
            _userIdentity = userIdentity;
            _mapper = mapper;
            emailService = new EmailService(_config);
            this.buildingContext = buildingContext;
            this.superOwnerContext = superOwnerContext;
        }

        public async Task<ResponseData<object>> CreateProject(CreateProjectDto request)
        {
            using (IDbContextTransaction transaction = superOwnerContext.Database.BeginTransaction())
            {
                try
                {
                    var project = new Project();
                    project.Id = Guid.NewGuid();
                    project.Position = request.Address;
                    project.Name = request.Name;
                    project.InvestorId = _userIdentity.UserId;

                    #region gen DB
                    transaction.CreateSavepoint("before_create_building");
                    string connectionTemplate = _config.GetConnectionString("BuildingTemplate");
                    var investorDbServerConfigs = _config.GetSection("MultiTenantDbSetting");
                    string connection = connectionTemplate;

                    connection = string.Format(connectionTemplate,
                        investorDbServerConfigs["Server"],
                        $"a_{project.Id}".Replace("-", "_"),
                        investorDbServerConfigs["User"],
                        investorDbServerConfigs["Password"]);

                    this.buildingContext.Database.SetConnectionString(connection);
                    this.buildingContext.Database.Migrate();
                    project.ConnectionString = connection;


                    UnitOfWork.ProjectRepository.Add(project);

                    await UnitOfWork.SaveChangesAsync();

                    transaction.ReleaseSavepoint("before_create_building");
                    transaction.Commit();
                    #endregion

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



            }
            //Investor investor = UnitOfWork.InvestorRepository.GetQuery(x => x.UserId == _userIdentity.UserId).FirstOrDefault();
            //if (investor == null) return new ResponseData<object>(ErrorCodeAPI.InvestorNotFound);

            return new ResponseData<object>()
            {
                Success = true,
                Message = MessConstant.Successfully,
                Code = (int)ErrorCodeAPI.OK,
            };
        }

        public async Task<ResponseData<object>> Delete(DeleteProjectRequest request)
        {
            Project project = UnitOfWork.ProjectRepository.GetQuery(x => x.Id == request.Id && x.InsertedById == _userIdentity.UserId).FirstOrDefault() as Project;
            if (project == null) return new ResponseData<object>(ErrorCodeAPI.ProjectNotFound);

            project.IsDeleted = true;
            UnitOfWork.ProjectRepository.Update(project);
            await UnitOfWork.SaveChangesAsync();

            return new ResponseData<object>()
            {
                Success = true,
                Message = MessConstant.Successfully,
                Code = (int)ErrorCodeAPI.OK,
            };
        }

        public async Task<ResponseData<object>> Update(UpdateProjectRequest request)
        {
            Project project = UnitOfWork.ProjectRepository.GetQuery(x => x.Id == request.Id && x.InsertedById == _userIdentity.UserId).FirstOrDefault() as Project;
            if (project == null) return new ResponseData<object>(ErrorCodeAPI.ProjectNotFound);

            project.Position = request.Address;
            project.Name = request.Name;

            UnitOfWork.ProjectRepository.Update(project);
            await UnitOfWork.SaveChangesAsync();

            return new ResponseData<object>()
            {
                Success = true,
                Message = MessConstant.Successfully,
                Code = (int)ErrorCodeAPI.OK,
            };
        }

        public async Task<ResponseData<List<ViewManagerProjectResponse>>> ViewManager(ViewManagerProjectRequest request)
        {
            List<ViewManagerProjectResponse> listViewManagerProject = new List<ViewManagerProjectResponse>();

            List<Project> projects = UnitOfWork.ProjectRepository.GetQuery(x => x.InsertedById == _userIdentity.UserId && x.IsActive && x.IsDeleted == false).ToList();
            foreach (var project in projects)
            {
                ViewManagerProjectResponse viewManagerProject = new ViewManagerProjectResponse();
                viewManagerProject.Address = project.Position;
                viewManagerProject.Name = project.Name;
                viewManagerProject.Id = project.Id;

                //List<Building> buildings = UnitOfWork.BuildingRepository.GetQuery(x => x.ProjectId == project.Id && x.IsActive && x.IsDeleted == false).ToList();
                //viewManagerProject.SoToaNha += buildings.Count();

                //foreach (var building in buildings)
                //{
                //    viewManagerProject.SlCuDan += building.MaxNumberResidents;
                //    viewManagerProject.SoCanHo += building.MaxNumberApartments;
                //}
                //listViewManagerProject.Add(viewManagerProject);
            }

            return new ResponseData<List<ViewManagerProjectResponse>>()
            {
                Success = true,
                Message = MessConstant.Successfully,
                Code = (int)ErrorCodeAPI.OK,
                Data = listViewManagerProject
            };
        }
    }
}
