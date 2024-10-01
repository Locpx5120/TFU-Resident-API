using AutoMapper;
using Constant;
using Core.Enums;
using Core.Handler;
using Core.Infrastructure;
using Core.Model;
using fake_tool.Helpers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using TFU_Resident_API.Model;

namespace Service.Impl
{
    public class AuthService : BaseHandler, IAuthService
    {
        private readonly IConfiguration _config;
        private readonly AppSettings _appSettings;
        private readonly IUserIdentity _userIdentity;
        private readonly IMapper _mapper;

        public AuthService(
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
        }

        public async Task<ResponseData<LoginResponseDto>> Login(LoginRequestDto request)
        {
            var jwt = new Jwt(_appSettings, _config);
            var user = await UnitOfWork.UserRepository.GetQuery(
                x => x.Email == request.Email &&
                x.Password == request.Password &&
                x.IsDeleted == false).FirstOrDefaultAsync();

            if (user == null) return new ResponseData<LoginResponseDto>(ErrorCodeAPI.BadRequest);

            var role = await UnitOfWork.RoleRepository.GetByIdAsync(user.RoleId);
            if (role == null) return new ResponseData<LoginResponseDto>(ErrorCodeAPI.BadRequest);

            var token = jwt.GenerateToken(user, role.Name ?? "");
            var response = new LoginResponseDto()
            {
                Token = token,
            };
            return new ResponseData<LoginResponseDto>()
            {
                Success = true,
                Message = MessConstant.Successfully,
                Code = (int)ErrorCodeAPI.OK,
                Data = response,
            };
        }

        public async Task<ResponseData<UserInfoResponse>> GetUserInfo()
        {
            var user = await UnitOfWork.UserRepository.GetQuery(
                f => f.Id == _userIdentity.UserId &&
                f.IsDeleted == false).FirstOrDefaultAsync();
            if (user == null) return new ResponseData<UserInfoResponse>(ErrorCodeAPI.BadRequest);

            var role = await UnitOfWork.RoleRepository.GetByIdAsync(user.RoleId);
            var response = _mapper.Map<UserInfoResponse>(user);
            response.RoleName = role?.Name;

            return new ResponseData<UserInfoResponse>()
            {
                Success = true,
                Message = MessConstant.Successfully,
                Code = (int)ErrorCodeAPI.OK,
                Data = response,
            };
        }

    }
}
