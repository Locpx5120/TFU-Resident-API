using BuildingModels;
using Constant;
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
                        Message = MessConstant.BuildingHasExist,
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
                    Address = request.Address,
                    CreateAt = request.CreateAt,
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
                    Message = MessConstant.BuildingUpdateSuccessfully,
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
                        Message = MessConstant.BuildingFoundZero,
                        Code = (int)ErrorCodeAPI.NotFound
                    };
                }

                // Cập nhật thông tin building
                existingBuilding.Name = request.BuildingName;
                existingBuilding.NumberFloor = request.NumberFloor;
                existingBuilding.NumberApartment = request.NumberApartment;
                existingBuilding.IsActive = request.IsActive;
                existingBuilding.Address = request.Address;
                existingBuilding.CreateAt = request.CreateAt;

                _unitOfWork.BuildingRepository.Update(existingBuilding);

                // Lưu thay đổi
                await _unitOfWork.SaveChangesAsync();

                // Trả về kết quả thành công
                return new ResponseData<BuildingUpdateResponseDto>
                {
                    Success = true,
                    Message = MessConstant.BuildingUpdateSuccessfully,
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

        public async Task<ResponseData<List<BuildingResponseDto>>> GetBuildingsAsync(String buildingName)
        {
            try
            {
                // Lấy danh sách các tòa nhà không bị xóa
                var buildings = _unitOfWork.BuildingRepository.GetQuery(x => x.IsDeleted == false)
                    .Select(b => new BuildingResponseDto
                    {
                        Id = b.Id,
                        BuildingName = b.Name,
                        CreateAt = b.CreateAt,
                        NumberFloor = (int)b.NumberFloor,
                        NumberApartment = (int)b.NumberApartment,
                        Address = b.Address,
                    }).ToList();
                if (!String.IsNullOrEmpty(buildingName))
                {
                    buildingName = buildingName.Trim();
                    if (!String.IsNullOrEmpty(buildingName))
                    {
                        buildings = buildings.Where(x => x.BuildingName.ToUpper().StartsWith(buildingName.ToUpper())).ToList();
                    }
                }

                List<Apartment> apartments = _unitOfWork.ApartmentRepository.GetQuery(x => buildings.Select(k => k.Id).ToList().Contains(x.BuildingId)).ToList();
                foreach (var item in buildings)
                {
                    if (!apartments.Any())
                    {
                        item.NumberOfCitizen = 0;
                        continue;
                    }
                    List<Apartment> apartmentBuilding = apartments.Where(x => x.BuildingId == item.Id).ToList();
                    if (!apartmentBuilding.Any())
                    {
                        item.NumberOfCitizen = 0;
                        continue;
                    }
                    List<OwnerShip> ownerShips = _unitOfWork.OwnerShipRepository.GetQuery(x => apartmentBuilding.Select(k => k.Id).ToList().Contains((Guid)x.ApartmentId)).ToList();
                    if (ownerShips.Any())
                    {
                        item.NumberOfCitizen += ownerShips.Count();
                        continue;
                    }
                    List<Living> livings = _unitOfWork.LivingRepository.GetQuery(x => apartmentBuilding.Select(k => k.Id).ToList().Contains((Guid)x.ApartmentId)).ToList();
                    if (livings.Any())
                    {
                        item.NumberOfCitizen += livings.Count();
                        continue;
                    }
                }



                return new ResponseData<List<BuildingResponseDto>>
                {
                    Success = true,
                    Message = MessConstant.FindSuccessfully,
                    Data = buildings ?? new List<BuildingResponseDto>(),
                    Code = (int)ErrorCodeAPI.OK
                };
            }
            catch (Exception ex)
            {
                return new ResponseData<List<BuildingResponseDto>>
                {
                    Success = false,
                    Message = $"Lỗi: {ex.Message}",
                    Code = (int)ErrorCodeAPI.SystemIsError
                };
            }
        }

        public async Task<ResponseData<List<BuildingResponseDto>>> GetBuildingsByUserIdAsync(Guid userId)
        {
            try
            {
                // Lấy danh sách các căn hộ mà user sở hữu từ bảng Ownerships
                var ownerships = await _unitOfWork.OwnerShipRepository
                    .GetQuery(o => o.ResidentId == userId && o.IsDeleted == false)
                    .Include(o => o.Apartment) // Bao gồm thông tin về căn hộ
                    .ThenInclude(a => a.Building) // Bao gồm thông tin về tòa nhà liên kết với căn hộ
                    .ToListAsync();

                if (!ownerships.Any())
                {
                    return new ResponseData<List<BuildingResponseDto>>
                    {
                        Success = false,
                        Message = MessConstant.UserNotApartment,
                        Code = (int)ErrorCodeAPI.NotFound
                    };
                }

                // Lấy danh sách tòa nhà từ các căn hộ mà user sở hữu
                var buildings = ownerships
                    .Select(o => o.Apartment.Building)
                    .Distinct() // Đảm bảo không có tòa nhà trùng lặp
                    .Select(b => new BuildingResponseDto
                    {
                        Id = b.Id,
                        BuildingName = b.Name,
                        CreateAt = b.CreateAt,
                        NumberFloor = (int)b.NumberFloor,
                        NumberApartment = (int)b.NumberApartment,
                        Address = b.Address,
                    })
                    .ToList();

                return new ResponseData<List<BuildingResponseDto>>
                {
                    Success = true,
                    Message = MessConstant.FindSuccessfully,
                    Data = buildings,
                    Code = (int)ErrorCodeAPI.OK
                };
            }
            catch (Exception ex)
            {
                return new ResponseData<List<BuildingResponseDto>>
                {
                    Success = false,
                    Message = $"Lỗi: {ex.Message}",
                    Code = (int)ErrorCodeAPI.SystemIsError
                };
            }
        }
    }
}
