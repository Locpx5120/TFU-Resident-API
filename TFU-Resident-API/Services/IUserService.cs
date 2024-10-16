using Core.Model;
using TFU_Resident_API.Dto;

namespace TFU_Resident_API.Services
{
    public interface IUserService
    {
        public Task<ResponseData<List<UserDto>>> ViewManager(ViewManagerUserRequest request);
        public Task<ResponseData<object>> Create(UserCreateRequest request);
        public Task<ResponseData<object>> Update(UserUpdateRequest request);
        public Task<ResponseData<object>> Delete(UserDeleteRequest request);
    }
}
