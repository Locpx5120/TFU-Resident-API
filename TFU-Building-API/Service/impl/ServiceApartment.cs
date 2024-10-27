using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TFU_Building_API.Dto;
using TFU_Building_API.Core.Infrastructure;
using Core.Model;
using Core.Enums;

namespace TFU_Building_API.Service.impl
{
    public class ServiceApartment : IServiceApartment
    {
        private readonly IUnitOfWork _unitOfWork;

        public ServiceApartment(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<ResponseData<PaginatedResponseDto<ApartmentServiceSummaryDto>>> GetApartmentServiceSummaryByUserId(Guid userId, int pageSize, int pageNumber)
        {
            try
            {
                var query = from sc in _unitOfWork.ServiceContractRepository.GetQuery(x => x.IsActive && x.IsDeleted == false)
                            join s in _unitOfWork.ServiceRepository.GetQuery(x => x.IsDeleted == false)
                                on sc.ServiceId equals s.Id
                            join o in _unitOfWork.OwnerShipRepository.GetQuery(x => x.UserId == userId && x.IsDeleted == false)
                                on sc.ApartmentId equals o.ApartmentId
                            group new { sc, s } by new { sc.ApartmentId, RoomNumber = sc.Apartment.RoomNumber } into apartmentGroup
                            select new ApartmentServiceSummaryDto
                            {
                                ApartmentId = apartmentGroup.Key.ApartmentId ?? new Guid(),
                                RoomNumber = apartmentGroup.Key.RoomNumber ?? 0,
                                TotalServices = apartmentGroup.Sum(x => x.s.Unit == "m2" ? 1 : x.sc.Quantity ?? 0)
                            };


                // Tính tổng số bản ghi trước khi phân trang
                var totalRecords = await query.CountAsync();

                // Áp dụng phân trang
                var data = await query
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();

                // Đóng gói kết quả vào PaginatedResponseDto
                var response = new PaginatedResponseDto<ApartmentServiceSummaryDto>
                {
                    TotalRecords = totalRecords,
                    Data = data
                };

                return new ResponseData<PaginatedResponseDto<ApartmentServiceSummaryDto>>
                {
                    Success = true,
                    Message = "Successfully retrieved apartment service summary.",
                    Data = response,
                    Code = (int)ErrorCodeAPI.OK
                };
            }
            catch (Exception ex)
            {
                return new ResponseData<PaginatedResponseDto<ApartmentServiceSummaryDto>>
                {
                    Success = false,
                    Message = ex.Message,
                    Code = (int)ErrorCodeAPI.SystemIsError
                };
            }
        }

        public async Task<ResponseData<List<ServiceDetailDto>>> GetServiceDetailsByApartmentId(ServiceDetailRequestDto request)
        {
            try
            {
                var query = from sc in _unitOfWork.ServiceContractRepository.GetQuery(x => x.ApartmentId == request.ApartmentId && x.IsActive && (x.IsDeleted == false))
                            join s in _unitOfWork.ServiceRepository.GetQuery(x => (x.IsDeleted == false))
                                on sc.ServiceId equals s.Id
                            join a in _unitOfWork.ApartmentRepository.GetQuery(x => (x.IsDeleted == false))
                                on sc.ApartmentId equals a.Id
                            join at in _unitOfWork.ApartmentTypeRepository.GetQuery(x => x.IsActive && (x.IsDeleted == false))
                                on a.ApartmentTypeId equals at.Id
                            select new ServiceDetailDto
                            {
                                ServiceName = s.ServiceName,
                                Description = s.Description,
                                QuantityOrArea = s.Unit == "m2" ? $"{at.LandArea} m2" : $"x{sc.Quantity}",
                                UnitPrice = s.UnitPrice,
                                TotalPrice = s.UnitPrice * (s.Unit == "m2" ? at.LandArea : sc.Quantity ?? 0),
                                StartDate = sc.StartDate??DateTime.Now,
                                EndDate = sc.EndDate ?? DateTime.Now
                            };

                // Áp dụng bộ lọc theo loại dịch vụ nếu có
                if (!string.IsNullOrEmpty(request.ServiceType))
                {
                    query = query.Where(x => x.ServiceName.Contains(request.ServiceType));
                }

                var data = await query.ToListAsync();

                return new ResponseData<List<ServiceDetailDto>>
                {
                    Success = true,
                    Message = "Successfully retrieved service details.",
                    Data = data,
                    Code = (int)ErrorCodeAPI.OK
                };
            }
            catch (Exception ex)
            {
                return new ResponseData<List<ServiceDetailDto>>
                {
                    Success = false,
                    Message = ex.Message,
                    Code = (int)ErrorCodeAPI.SystemIsError
                };
            }
        }
    }
}
