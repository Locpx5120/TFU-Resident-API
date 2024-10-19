using AutoMapper;
using BuildingModels;
using Constant;
using Core.Enums;
using Core.Model;

using fake_tool.Helpers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using TFU_Building_API.Core.Handler;
using TFU_Building_API.Core.Helper;
using TFU_Building_API.Core.Infrastructure;
using TFU_Building_API.Helpers;
using TFU_Building_API.Model;
using TFU_Resident_API.Constant;
using AppSettings = TFU_Building_API.Core.Struct.AppSetting.AppSettings;

namespace TFU_Building_API.Service.Impl
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


            var token = jwt.GenerateToken(user, role.Name ?? "", request.BuildingId ?? Guid.Empty);
            var response = new LoginResponseDto()
            {
                Token = token,
            };

            if (user.IsChangePassword == true)
            {
                return new ResponseData<LoginResponseDto>()
                {
                    Success = true,
                    Message = MessConstant.Successfully,
                    Code = (int)ErrorCodeAPI.OKChangePass,
                    Data = response,
                };
            }

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
            //var customerCheck = await UnitOfWork.InvestorRepository.GetQuery(x =>
            //x.InvestorName == register.CompanyName
            //).FirstOrDefaultAsync();

            //if (customerCheck != null) return new ResponseData<RegisterResponseDto>(ErrorCodeAPI.CustomerUsed);

            //var userCheck = await UnitOfWork.UserRepository.GetQuery(x =>
            //x.Email == register.Email).FirstOrDefaultAsync();
            //if (userCheck != null) return new ResponseData<RegisterResponseDto>(ErrorCodeAPI.EmailUsed);

            string userName = register.Email.Split('@')[0];
            if (String.IsNullOrEmpty(userName)) return new ResponseData<RegisterResponseDto>(ErrorCodeAPI.EmailNotAvailable);

            User user = new User();
            user.Id = Guid.NewGuid();
            user.FullName = userName;
            user.Email = register.Email;
            user.Password = Utill.GeneratePassword();
            user.PhoneNumber = register.Phone;
            user.RoleId = Guid.Parse("98AE41E1-3379-4193-9856-1C9162A8C9C2"); //User
            user.IsChangePassword = true;
            UnitOfWork.UserRepository.Add(user);

            await UnitOfWork.SaveChangesAsync();

            await emailService.SendEmailAsync(user.Email, "TB Dki tai khoan", BodyMaillRegister(user));

            return new ResponseData<RegisterResponseDto>(ErrorCodeAPI.OK);
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
            <p>Dear <span class='highlight'>{user.Username}</span>,</p>
            <p>Thank you for registering with our service. Below are your account details:</p>
            <p><strong>Username:</strong> <span class='highlight'>{user.Username}</span></p>
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

        private string BodyMaillForgot(User user, String otp)
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
            <h1>Bạn đã thực hiện yêu cầu quên mật khẩu!</h1>
        </div>
        <div class='content'>
            <p>Dear <span class='highlight'>{user.Username}</span>,</p>
            <p>Thông tin xác thức của bạn:</p>
            <p><strong>OTP:</strong> <span class='highlight'>{otp}</span></p>
            <p>Vui lòng giữ thông tin này an toàn và không chia sẻ với bất kỳ ai. Bạn có thể đăng nhập vào tài khoản của mình bằng thông tin đăng nhập được cung cấp.</p>
        </div>
        <div class='footer'>
            <p>&copy; 2024 Our Company. All rights reserved.</p>
        </div>
    </div>
</body>
</html>";

            return emailBody;
        }

        private string BodyMaillForgotOk(User user)
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
            <h1>Bạn đã thực hiện xác minh quên mật khẩu!</h1>
        </div>
        <div class='content'>
            <p>Dear <span class='highlight'>{user.Username}</span>,</p>
            <p>Thông tin xác thức của bạn:</p>
            <p><strong>Mật khẩu:</strong> <span class='highlight'>{user.Password}</span></p>
            <p>Vui lòng giữ thông tin này an toàn và không chia sẻ với bất kỳ ai. Bạn có thể đăng nhập vào tài khoản của mình bằng thông tin đăng nhập được cung cấp.</p>
        </div>
        <div class='footer'>
            <p>&copy; 2024 Our Company. All rights reserved.</p>
        </div>
    </div>
</body>
</html>";

            return emailBody;
        }

        public async Task<ResponseData<ForgotPasswordResponseDto>> ForgotPassword(ForgotPasswordRequestDto request)
        {
            var userCheck = await UnitOfWork.UserRepository.GetQuery(x =>
            x.Email == request.Email).FirstOrDefaultAsync();
            if (userCheck == null) return new ResponseData<ForgotPasswordResponseDto>(ErrorCodeAPI.EmailNotUse);

            BuildingModels.OTPMail oTPMail = new BuildingModels.OTPMail();
            oTPMail.UserId = userCheck.Id;
            oTPMail.Otp = Utill.GenerateRandomInt(6);
            oTPMail.ContentMail = BodyMaillForgot(userCheck, oTPMail.Otp);
            oTPMail.EffectiveDate = DateTime.Now.AddMinutes(15);
            oTPMail.TypeOtp = TypeOtp.RESET_PASSWORD;

            await emailService.SendEmailAsync(userCheck.Email, "Mã xác nhận quên mật khẩu", oTPMail.ContentMail);

            UnitOfWork.OTPMailRepository.Add(oTPMail);

            if (await UnitOfWork.SaveChangesAsync() > 0)
            {
                return new ResponseData<ForgotPasswordResponseDto>()
                {
                    Success = true,
                    Message = MessConstant.Successfully,
                    Code = (int)ErrorCodeAPI.OK,
                    Data = new ForgotPasswordResponseDto { UserId = userCheck.Id },
                };
            }

            return new ResponseData<ForgotPasswordResponseDto>()
            {
                Success = true,
                Message = MessConstant.Failed,
                Code = (int)ErrorCodeAPI.SystemIsError,
                Data = null,
            };
        }

        public async Task<ResponseData<ChangePasswordResponseDto>> ChangePassword(ChangePasswordRequestDto request)
        {
            var user = await UnitOfWork.UserRepository.GetQuery(x => x.Id == _userIdentity.UserId).FirstOrDefaultAsync();
            if (user == null) return new ResponseData<ChangePasswordResponseDto>(ErrorCodeAPI.UserNotExit);

            (bool isValid, string mess) = Utill.ValidatePassword(request.Password);
            if (!isValid)
            {
                return new ResponseData<ChangePasswordResponseDto>()
                {
                    Success = true,
                    Message = mess,
                    Code = (int)ErrorCodeAPI.PasswordIsNotValid,
                    Data = null,
                };
            }

            (isValid, mess) = Utill.ValidatePassword(request.NewPassword);
            if (!isValid)
            {
                return new ResponseData<ChangePasswordResponseDto>()
                {
                    Success = true,
                    Message = mess,
                    Code = (int)ErrorCodeAPI.NewPasswordIsNotValid,
                    Data = null,
                };
            }

            //kiểm tra mật khẩu cũ
            if (!user.Password.Equals(request.Password))
            {
                return new ResponseData<ChangePasswordResponseDto>()
                {
                    Success = true,
                    Message = MessConstant.Failed,
                    Code = (int)ErrorCodeAPI.PasswordIsNotCorrect,
                    Data = null,
                };
            }

            user.Password = request.NewPassword;
            user.IsChangePassword = false;
            UnitOfWork.UserRepository.Update(user);

            if (await UnitOfWork.SaveChangesAsync() > 0)
            {
                return new ResponseData<ChangePasswordResponseDto>()
                {
                    Success = true,
                    Message = MessConstant.Successfully,
                    Code = (int)ErrorCodeAPI.OK,
                    Data = null,
                };
            }

            return new ResponseData<ChangePasswordResponseDto>()
            {
                Success = true,
                Message = MessConstant.Failed,
                Code = (int)ErrorCodeAPI.SystemIsError,
                Data = null,
            };
        }

        public async Task<ResponseData<ConfirmOtpResponseDto>> ConfirmOtp(ConfirmOtpRequestDto request)
        {
            var user = await UnitOfWork.UserRepository.GetQuery(x => x.Id == request.UserId).FirstOrDefaultAsync();
            if (user == null) return new ResponseData<ConfirmOtpResponseDto>(ErrorCodeAPI.UserNotExit);

            BuildingModels.OTPMail oTPMail = await UnitOfWork.OTPMailRepository.GetQuery(x => x.UserId == request.UserId
            && x.TypeOtp == request.TypeOtp
            && x.IsActive == true
            && x.Otp == request.Otp
            && x.EffectiveDate >= DateTime.Now).FirstOrDefaultAsync();

            if (oTPMail == null) return new ResponseData<ConfirmOtpResponseDto>(ErrorCodeAPI.OtpNotExit);

            if (oTPMail.TypeOtp == TypeOtp.RESET_PASSWORD)
            {
                user.Password = Utill.GeneratePassword(8);
                user.IsChangePassword = true;
                UnitOfWork.UserRepository.Update(user);

                await emailService.SendEmailAsync(user.Email, "Reset mật khẩu", BodyMaillForgotOk(user));
            }

            oTPMail.IsActive = false;

            UnitOfWork.OTPMailRepository.Update(oTPMail);

            if (await UnitOfWork.SaveChangesAsync() > 0)
            {
                return new ResponseData<ConfirmOtpResponseDto>()
                {
                    Success = true,
                    Message = MessConstant.Successfully,
                    Code = (int)ErrorCodeAPI.OK,
                    Data = null,
                };
            }

            return new ResponseData<ConfirmOtpResponseDto>()
            {
                Success = true,
                Message = MessConstant.Failed,
                Code = (int)ErrorCodeAPI.SystemIsError,
                Data = null,
            };
        }
    }
}
