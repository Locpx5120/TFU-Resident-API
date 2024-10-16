using AutoMapper;
using BuildingModels;
using Constant;
using Core.Enums;
using Core.Handler;
using Core.Infrastructure;
using Core.Model;
using Entity;
using fake_tool.Helpers;
using Microsoft.Extensions.Options;
using TFU_Resident_API.Core.Helper;
using TFU_Resident_API.Data;
using TFU_Resident_API.Dto;
using RoleEntity = Entity.Role;

namespace TFU_Resident_API.Services.Impl
{
    public class UserService : BaseHandler, IUserService
    {
        private readonly IConfiguration _config;
        private readonly AppSettings _appSettings;
        private readonly IUserIdentity _userIdentity;
        private readonly IMapper _mapper;
        private readonly EmailService emailService;

        private readonly AppDbContext superOwnerContext;
        private readonly BuildingContext buildingContext;

        public UserService(
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

        public async Task<ResponseData<object>> Create(UserCreateRequest request)
        {
            var user = this._mapper.Map<User>(request);
            if (user == null) return new ResponseData<object>(ErrorCodeAPI.UserNotFound);

            user.Password = Utill.GeneratePassword(8);
            user.IsChangePassword = true;

            UnitOfWork.UserRepository.Add(user);
            await emailService.SendEmailAsync(user.Email, "TB Dki tai khoan", BodyMaillRegister(user));

            if (await UnitOfWork.SaveChangesAsync() > 0)
            {
                return new ResponseData<object>()
                {
                    Success = true,
                    Message = MessConstant.Successfully,
                    Code = (int)ErrorCodeAPI.OK,
                };
            }

            return new ResponseData<object>()
            {
                Success = true,
                Message = MessConstant.Failed,
                Code = (int)ErrorCodeAPI.SystemIsError,
            };
        }

        private string BodyMaillRegister(User user)
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

        public async Task<ResponseData<object>> Delete(UserDeleteRequest request)
        {
            var user = UnitOfWork.UserRepository.GetQuery(x => x.Id == request.Id).FirstOrDefault();
            if (user == null) return new ResponseData<object>(ErrorCodeAPI.UserNotFound);

            user.IsDeleted = true;

            UnitOfWork.UserRepository.Update(user);

            if (await UnitOfWork.SaveChangesAsync() > 0)
            {
                return new ResponseData<object>()
                {
                    Success = true,
                    Message = MessConstant.Successfully,
                    Code = (int)ErrorCodeAPI.OK,
                };
            }

            return new ResponseData<object>()
            {
                Success = true,
                Message = MessConstant.Failed,
                Code = (int)ErrorCodeAPI.SystemIsError,
            };
        }

        public async Task<ResponseData<object>> Update(UserUpdateRequest request)
        {
            var user = UnitOfWork.UserRepository.GetQuery(x => x.Id == request.Id).FirstOrDefault();
            if (user == null) return new ResponseData<object>(ErrorCodeAPI.UserNotFound);

            user.Email = request.Email;
            user.Phone = request.Phone;
            user.RoleId = request.RoleId;
            user.UserName = request.UserName;
            user.Dob = request.Dob;
            user.NumberCccd = request.NumberCccd;
            user.Gender = request.Genders.Equals("Male");

            UnitOfWork.UserRepository.Update(user);

            if (await UnitOfWork.SaveChangesAsync() > 0)
            {
                return new ResponseData<object>()
                {
                    Success = true,
                    Message = MessConstant.Successfully,
                    Code = (int)ErrorCodeAPI.OK,
                };
            }

            return new ResponseData<object>()
            {
                Success = true,
                Message = MessConstant.Failed,
                Code = (int)ErrorCodeAPI.SystemIsError,
            };
        }

        public async Task<ResponseData<List<UserDto>>> ViewManager(ViewManagerUserRequest request)
        {
            List<User> users = UnitOfWork.UserRepository
                .GetQuery(x => x.IsActive == true && x.IsDeleted == false).ToList();

            if (!String.IsNullOrEmpty(request.Name))
            {
                users = users.Where(x => x.UserName == request.Name).ToList();
            }

            if (request.RoleId != null && request.RoleId != Guid.Empty)
            {
                RoleEntity role = UnitOfWork.RoleRepository.GetQuery(role => role.Id == request.RoleId).FirstOrDefault();
                if (role == null) return new ResponseData<List<UserDto>>(ErrorCodeAPI.RoleNotFound);

                if (role != null)
                    users = users.Where(x => x.RoleId == role.Id).ToList();
            }

            var userDto = this._mapper.Map<List<UserDto>>(users);

            List<RoleEntity> roles = UnitOfWork.RoleRepository.GetQuery(x => x.IsActive == true && x.IsDeleted == false).ToList();

            foreach (var item in userDto)
            {
                if (item.RoleId != null)
                {
                    item.RoleName = roles.FirstOrDefault(x => x.Id == item.RoleId).Name;
                }
                else
                {
                    item.RoleName = "";
                }
            }

            return new ResponseData<List<UserDto>>()
            {
                Success = true,
                Message = MessConstant.Successfully,
                Code = (int)ErrorCodeAPI.OK,
                Data = userDto
            };
        }
    }
}
