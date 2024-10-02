using AutoMapper;
using Constant;
using Core.Enums;
using Core.Handler;
using Core.Infrastructure;
using Core.Model;
using Entity;
using fake_tool.Helpers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using TFU_Resident_API.Core.Helper;
using TFU_Resident_API.Entity;
using TFU_Resident_API.Model;

namespace Service.Impl
{
    public class AuthService : BaseHandler, IAuthService
    {
        private readonly IConfiguration _config;
        private readonly AppSettings _appSettings;
        private readonly IUserIdentity _userIdentity;
        private readonly IMapper _mapper;
        private readonly EmailService emailService;

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
            emailService = new EmailService(_config);
        }

        public async Task<ResponseData<LoginResponseDto>> Login(LoginRequestDto request)
        {
            var jwt = new Jwt(_appSettings, _config);
            var user = await UnitOfWork.UserRepository.GetQuery(
                x => x.Email == request.Email &&
                x.Password == request.Password &&
                x.IsActive == true &&
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

        public async Task<ResponseData<RegisterResponseDto>> Register(RegisterRequestDto register)
        {
            var customerCheck = await UnitOfWork.CustomerRepository.GetQuery(x =>
            x.Name == register.CompanyName
            ).FirstOrDefaultAsync();

            if (customerCheck != null) return new ResponseData<RegisterResponseDto>(ErrorCodeAPI.CustomerUsed);

            var userCheck = await UnitOfWork.UserRepository.GetQuery(x =>
            x.Email == register.Email).FirstOrDefaultAsync();
            if (userCheck != null) return new ResponseData<RegisterResponseDto>(ErrorCodeAPI.EmailUsed);

            string userName = register.Email.Split('@')[0];
            if (String.IsNullOrEmpty(userName)) return new ResponseData<RegisterResponseDto>(ErrorCodeAPI.EmailNotAvailable);

            User user = new User();
            user.Id = Guid.NewGuid();
            user.UserName = userName;
            user.Email = register.Email;
            user.Password = register.Password;
            user.Phone = register.Phone;
            user.RoleId = Guid.Parse("98AE41E1-3379-4193-9856-1C9162A8C9C2"); //User
            UnitOfWork.UserRepository.Add(user);

            Customer customer = new Customer();
            customer.Name = register.CompanyName;
            customer.StartDate = DateTime.Now;
            customer.CodePostion = "";
            customer.UserId = user.Id;
            UnitOfWork.CustomerRepository.Add(customer);

            UnitOfWork.SaveChangesAsync();

            emailService.SendEmailAsync("recipient@example.com", "TB Dki tai khoan", BodyMaillRegister(user, customer));

            return new ResponseData<RegisterResponseDto>(ErrorCodeAPI.OK);
        }

        private string BodyMaillRegister(User user, Customer customer)
        {
            string emailBody = $@"
<!DOCTYPE html>
<html lang='en'>
<head>
    <meta charset='UTF-8'>
    <meta http-equiv='X-UA-Compatible' content='IE=edge'>
    <meta name='viewport' content='width=device-width, initial-scale=1.0'>
    <style>
        body {{
            font-family: Arial, sans-serif;
            background-color: #f4f4f4;
            margin: 0;
            padding: 0;
        }}
        .container {{
            background-color: #ffffff;
            width: 100%;
            max-width: 600px;
            margin: 20px auto;
            padding: 20px;
            border-radius: 10px;
            box-shadow: 0 4px 8px rgba(0, 0, 0, 0.1);
        }}
        .header {{
            background-color: #4CAF50;
            padding: 10px;
            text-align: center;
            border-radius: 10px 10px 0 0;
            color: white;
        }}
        .content {{
            margin: 20px 0;
            text-align: left;
        }}
        .content p {{
            font-size: 16px;
            color: #333333;
        }}
        .content .highlight {{
            color: #4CAF50;
            font-weight: bold;
        }}
        .footer {{
            text-align: center;
            margin-top: 20px;
            font-size: 12px;
            color: #aaaaaa;
        }}
    </style>
</head>
<body>
    <div class='container'>
        <div class='header'>
            <h1>Welcome to Our Service!</h1>
        </div>
        <div class='content'>
            <p>Dear <span class='highlight'>{user.UserName}</span>,</p>
            <p>Thank you for registering with our service. Below are your account details:</p>
            <p><strong>Username:</strong> <span class='highlight'>{user.UserName}</span></p>
            <p><strong>Password:</strong> <span class='highlight'>{user.Password}</span></p>
            <p>Please keep this information safe and do not share it with anyone. You can log in to your account using the credentials provided.</p>
            <p>If you have any questions, feel free to contact us at any time.</p>
            <p>Best regards,<br>The Support Team</p>
        </div>
        <div class='footer'>
            <p>&copy; 2024 Our Company. All rights reserved.</p>
        </div>
    </div>
</body>
</html>";

            return emailBody;
        }
    }
}
