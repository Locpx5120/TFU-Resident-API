using Core.Model;
using TFU_Resident_API.Model;

namespace Service
{
    public interface IAuthService
    {
        public Task<ResponseData<LoginResponseDto>> Login(LoginRequestDto request);
        public Task<ResponseData<UserInfoResponse>> GetUserInfo();
        public Task<ResponseData<RegisterResponseDto>> Register(RegisterRequestDto register);
    }
}
