using BuildingModels;
using Core.Enums;
using Core.Model;
using Microsoft.EntityFrameworkCore;
using TFU_Building_API.Core.Infrastructure;
using TFU_Building_API.Dto;

namespace TFU_Building_API.Service.impl
{
    public class BuildingService : IBuildingService
    {
        private readonly IUnitOfWork _unitOfWork;

        public BuildingService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<ResponseData<BuildingResponseDto>> AddBuilding(BuildingRequestDto request)
        {
            try
            {
                // Kiểm tra xem tên tòa nhà đã tồn tại chưa
                var existingBuilding = await _unitOfWork.BuildingRepository.GetQuery(x => x.Name == request.BuildingName && x.IsDeleted == false).FirstOrDefaultAsync();

                if (existingBuilding != null)
                {
                    return new ResponseData<BuildingResponseDto>
                    {
                        Success = false,
                        Message = "Building already exists.",
                        Code = (int)ErrorCodeAPI.DuplicateEntry
                    };
                }

                // Tạo tòa nhà mới
                var newBuilding = new Building
                {
                    Id = Guid.NewGuid(),
                    Name = request.BuildingName,
                    NumberFloor = request.NumberFloor,
                    NumberApartment = request.NumberApartment,
                    IsDeleted = false,
                    IsActive = true,
                    InsertedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now,
                };

                _unitOfWork.BuildingRepository.Add(newBuilding);  // Thêm vào cơ sở dữ liệu
                await _unitOfWork.SaveChangesAsync();             // Lưu thay đổi

                var response = new BuildingResponseDto
                {
                    Id = newBuilding.Id,
                    BuildingName = newBuilding.Name
                };

                return new ResponseData<BuildingResponseDto>
                {
                    Success = true,
                    Message = "Building created successfully.",
                    Data = response,
                    Code = (int)ErrorCodeAPI.OK
                };
            }
            catch (Exception ex)
            {
                return new ResponseData<BuildingResponseDto>
                {
                    Success = false,
                    Message = ex.Message,
                    Code = (int)ErrorCodeAPI.SystemIsError
                };
            }
        }

        public async Task<ResponseData<BuildingUpdateResponseDto>> UpdateBuilding(BuildingUpdateRequestDto request)
        {
            try
            {
                // Tìm kiếm building dựa trên ID
                var existingBuilding = await _unitOfWork.BuildingRepository.GetQuery(x => x.Id == request.Id && x.IsDeleted == false).FirstOrDefaultAsync();

                if (existingBuilding == null)
                {
                    // Nếu không tìm thấy building, trả về lỗi
                    return new ResponseData<BuildingUpdateResponseDto>
                    {
                        Success = false,
                        Message = "Building not found.",
                        Code = (int)ErrorCodeAPI.NotFound
                    };
                }

                // Cập nhật thông tin building
                existingBuilding.Name = request.BuildingName;
                existingBuilding.NumberFloor = request.NumberFloor;
                existingBuilding.NumberApartment = request.NumberApartment;
                existingBuilding.IsActive = request.IsActive;
                existingBuilding.UpdatedAt = DateTime.Now;

                // Lưu thay đổi
                await _unitOfWork.SaveChangesAsync();

                // Trả về kết quả thành công
                return new ResponseData<BuildingUpdateResponseDto>
                {
                    Success = true,
                    Message = "Building updated successfully.",
                    Data = new BuildingUpdateResponseDto { Id = existingBuilding.Id },
                    Code = (int)ErrorCodeAPI.OK
                };
            }
            catch (Exception ex)
            {
                // Xử lý lỗi
                return new ResponseData<BuildingUpdateResponseDto>
                {
                    Success = false,
                    Message = ex.Message,
                    Code = (int)ErrorCodeAPI.SystemIsError
                };
            }
        }

        public async Task<ResponseData<List<BuildingResponseDto>>> GetBuildingsAsync()
        {
            try
            {
                // Lấy danh sách các tòa nhà không bị xóa
                var query = _unitOfWork.BuildingRepository.GetQuery(x => x.IsDeleted == false)
                    .Select(b => new BuildingResponseDto
                    {
                        Id = b.Id,
                        BuildingName = b.Name,
                       
                    });

                var buildings = await query.ToListAsync();

                return new ResponseData<List<BuildingResponseDto>>
                {
                    Success = true,
                    Message = "Successfully retrieved buildings.",
                    Data = buildings,
                    Code = (int)ErrorCodeAPI.OK
                };
            }
            catch (Exception ex)
            {
                return new ResponseData<List<BuildingResponseDto>>
                {
                    Success = false,
                    Message = $"An error occurred: {ex.Message}",
                    Code = (int)ErrorCodeAPI.SystemIsError
                };
            }
        }

    }
}
