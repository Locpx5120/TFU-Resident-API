using Core.Model;
using TFU_Building_API.Model;

namespace TFU_Building_API.Service
{
    public interface IAuthService
    {
        public Task<ResponseData<LoginResponseDto>> Login(LoginRequestDto request);
        public Task<ResponseData<UserInfoResponse>> GetUserInfo();
        public Task<ResponseData<RegisterResponseDto>> Register(RegisterRequestDto register);
        public Task<ResponseData<ChangePasswordResponseDto>> ChangePassword(ChangePasswordRequestDto request);
        public Task<ResponseData<ForgotPasswordResponseDto>> ForgotPassword(ForgotPasswordRequestDto register);
        public Task<ResponseData<ConfirmOtpResponseDto>> ConfirmOtp(ConfirmOtpRequestDto request);
    }
}
