using TFU_Building_API.Dto;

namespace TFU_Building_API.Core.Dapper.User
{
    public interface IUserRepository
    {
        public Task<IEnumerable<GetStaffAssigmentResponseDto>> GetStaffListAssigment(string searchName);
    }
}
