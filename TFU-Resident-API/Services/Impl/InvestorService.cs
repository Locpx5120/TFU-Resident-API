using AutoMapper;
using Constant;
using Core.Enums;
using Core.Handler;
using Core.Infrastructure;
using Core.Model;
using fake_tool.Helpers;
using Microsoft.Extensions.Options;
using SuperOwnerModels;
using TFU_Resident_API.Core.Helper;
using TFU_Resident_API.Dto;

namespace TFU_Resident_API.Services.Impl
{
    public class InvestorService : BaseHandler, IInvestorService
    {
        private readonly IConfiguration _config;
        private readonly AppSettings _appSettings;
        private readonly IUserIdentity _userIdentity;
        private readonly IMapper _mapper;
        private readonly EmailService emailService;

        public InvestorService(
            IOptionsMonitor<AppSettings> optionsMonitor,
            IConfiguration config,
            IUnitOfWork unitOfWork,
            IHttpContextAccessor httpContextAccessor,
            IUserIdentity userIdentity,
            IMapper mapper
        ) : base(unitOfWork, httpContextAccessor)
        {
            _appSettings = optionsMonitor.CurrentValue;
            _config = config;
            _userIdentity = userIdentity;
            _mapper = mapper;
            emailService = new EmailService(_config);
        }

        public async Task<ResponseData<object>> CreateInvestor(CreateInvestorDto request)
        {
            var investor = new Investor()
            {
                UserId = request.UserId,
            };
            UnitOfWork.InvestorRepository.Add(investor);

            await UnitOfWork.SaveChangesAsync();

            return new ResponseData<object>()
            {
                Success = true,
                Message = MessConstant.Successfully,
                Code = (int)ErrorCodeAPI.OK,
            };
        }

        public async Task<ResponseData<ViewManagerResponse>> ViewManager()
        {
            Investor investor = UnitOfWork.InvestorRepository.GetQuery(x => x.UserId == _userIdentity.UserId && x.IsActive && x.IsDeleted == false).FirstOrDefault();
            if (investor == null) return new ResponseData<ViewManagerResponse>(ErrorCodeAPI.InvestorNotFound);

            ViewManagerResponse viewManagerResponse = new ViewManagerResponse();

            List<Project> projects = UnitOfWork.ProjectRepository
                .GetQuery(x => x.InvestorId == investor.Id && x.IsActive && x.IsDeleted == false).ToList();
            foreach (var project in projects)
            {
                List<Building> buildings = UnitOfWork.BuildingRepository
                    .GetQuery(x => x.ProjectId == project.Id && x.IsActive && x.IsDeleted == false).ToList();
                viewManagerResponse.DsToaNha += buildings.Count();

                foreach (var building in buildings)
                {
                    viewManagerResponse.DsCuDan += building.MaxNumberResidents;
                    viewManagerResponse.DsCanHo += building.MaxNumberApartments;
                }
            }

            viewManagerResponse.DsDuAn = projects.Count();

            return new ResponseData<ViewManagerResponse>()
            {
                Success = true,
                Message = MessConstant.Successfully,
                Code = (int)ErrorCodeAPI.OK,
                Data = viewManagerResponse
            };
        }
    }
}
