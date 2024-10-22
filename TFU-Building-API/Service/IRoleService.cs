using Core.Model;
using TFU_Building_API.Dto;

namespace TFU_Building_API.Service
{
    public interface IRoleService
    {
        Task<ResponseData<RoleResponseDto>> AddRole(RoleRequestDto request);

        Task<ResponseData<List<RoleResponseDto>>> GetRoles();
    }

}
