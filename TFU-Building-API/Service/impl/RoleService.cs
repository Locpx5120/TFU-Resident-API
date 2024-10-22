using BuildingModels;
using Core.Enums;
using Core.Model;
using Microsoft.EntityFrameworkCore;
using TFU_Building_API.Core.Infrastructure;
using TFU_Building_API.Dto;

namespace TFU_Building_API.Service.impl
{
    public class RoleService : IRoleService
    {
        private readonly IUnitOfWork _unitOfWork;

        public RoleService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<ResponseData<RoleResponseDto>> AddRole(RoleRequestDto request)
        {
            try
            {
                // Kiểm tra xem vai trò đã tồn tại hay chưa (dựa trên tên vai trò)
                var existingRole = await _unitOfWork.RoleRepository.GetQuery(x => x.Name == request.Name && x.IsDeleted == false).FirstOrDefaultAsync();

                if (existingRole != null)
                {
                    return new ResponseData<RoleResponseDto>
                    {
                        Success = false,
                        Message = "Role already exists.",
                        Code = (int)ErrorCodeAPI.DuplicateEntry
                    };
                }

                // Tạo vai trò mới
                var newRole = new Role
                {
                    Name = request.Name,
                    IsDeleted = false,
                    IsActive = true,
                    InsertedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now,
                };

                _unitOfWork.RoleRepository.Add(newRole);
                await _unitOfWork.SaveChangesAsync();

                var response = new RoleResponseDto
                {
                    Id = newRole.Id,
                    Name = newRole.Name
                };

                return new ResponseData<RoleResponseDto>
                {
                    Success = true,
                    Message = "Role added successfully.",
                    Data = response,
                    Code = (int)ErrorCodeAPI.OK
                };
            }
            catch (Exception ex)
            {
                // Xử lý lỗi và trả về lỗi hệ thống
                return new ResponseData<RoleResponseDto>
                {
                    Success = false,
                    Message = ex.Message,
                    Code = (int)ErrorCodeAPI.SystemIsError
                };
            }
        }

        public async Task<ResponseData<List<RoleResponseDto>>> GetRoles()
        {
            try
            {
                // Truy vấn lấy tất cả các role không bị xóa
                var roles = await _unitOfWork.RoleRepository.GetQuery(x => x.IsDeleted == false)
                    .Select(role => new RoleResponseDto
                    {
                        Id = role.Id,
                        Name = role.Name
                    })
                    .ToListAsync();

                return new ResponseData<List<RoleResponseDto>>
                {
                    Success = true,
                    Message = "Roles retrieved successfully.",
                    Data = roles,
                    Code = (int)ErrorCodeAPI.OK
                };
            }
            catch (Exception ex)
            {
                // Xử lý lỗi nếu có
                return new ResponseData<List<RoleResponseDto>>
                {
                    Success = false,
                    Message = ex.Message,
                    Code = (int)ErrorCodeAPI.SystemIsError
                };
            }
        }
    }
}
