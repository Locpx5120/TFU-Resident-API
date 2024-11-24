using Core.Enums;
using Core.Model;
using Microsoft.EntityFrameworkCore;
using TFU_Building_API.Core.Infrastructure;
using TFU_Building_API.Dto;

namespace TFU_Building_API.Service.impl
{
    public class ServiceApartment : IServiceApartment
    {
        private readonly IUnitOfWork _unitOfWork;

        public ServiceApartment(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        //public async Task<ResponseData<PaginatedResponseDto<ApartmentServiceSummaryDto>>> GetApartmentServiceSummaryByUserId(Guid userId, int pageSize, int pageNumber)
        //{
        //    try
        //    {
        //        var query = from sc in _unitOfWork.ServiceContractRepository.GetQuery(x => x.IsActive && x.IsDeleted == false)
        //                    join s in _unitOfWork.ServiceRepository.GetQuery(x => x.IsDeleted == false)
        //                        on sc.ServiceId equals s.Id
        //                    join o in _unitOfWork.OwnerShipRepository.GetQuery(x => x.ResidentId == userId && x.IsDeleted == false)
        //                        on sc.ApartmentId equals o.ApartmentId
        //                    group new { sc, s } by new { sc.ApartmentId, RoomNumber = sc.Apartment.RoomNumber } into apartmentGroup
        //                    select new ApartmentServiceSummaryDto
        //                    {
        //                        ApartmentId = apartmentGroup.Key.ApartmentId ?? new Guid(),
        //                        RoomNumber = apartmentGroup.Key.RoomNumber,
        //                        TotalServices = apartmentGroup.Sum(x => x.s.Unit == "m2" ? 1 : x.sc.Quantity ?? 0)
        //                    };


        //        // Tính tổng số bản ghi trước khi phân trang
        //        var totalRecords = await query.CountAsync();

        //        // Áp dụng phân trang
        //        var data = await query
        //            .Skip((pageNumber - 1) * pageSize)
        //            .Take(pageSize)
        //            .ToListAsync();

        //        // Đóng gói kết quả vào PaginatedResponseDto
        //        var response = new PaginatedResponseDto<ApartmentServiceSummaryDto>
        //        {
        //            TotalRecords = totalRecords,
        //            Data = data
        //        };

        //        return new ResponseData<PaginatedResponseDto<ApartmentServiceSummaryDto>>
        //        {
        //            Success = true,
        //            Message = "Successfully retrieved apartment service summary.",
        //            Data = response,
        //            Code = (int)ErrorCodeAPI.OK
        //        };
        //    }
        //    catch (Exception ex)
        //    {
        //        return new ResponseData<PaginatedResponseDto<ApartmentServiceSummaryDto>>
        //        {
        //            Success = false,
        //            Message = ex.Message,
        //            Code = (int)ErrorCodeAPI.SystemIsError
        //        };
        //    }
        //}

        public async Task<ResponseData<PaginatedResponseDto<ApartmentServiceSummaryDto>>> GetApartmentServiceSummaryByUserId(Guid userId, int pageSize, int pageNumber)
        {
            try
            {
                var query = from sc in _unitOfWork.ServiceContractRepository.GetQuery(x => x.IsActive && x.IsDeleted == false)
                            join s in _unitOfWork.ServiceRepository.GetQuery(x => x.IsDeleted == false)
                                on sc.ServiceId equals s.Id
                            join o in _unitOfWork.OwnerShipRepository.GetQuery(x => x.ResidentId == userId && x.IsDeleted == false)
                                on sc.ApartmentId equals o.ApartmentId
                            join a in _unitOfWork.ApartmentRepository.GetQuery(x => x.IsDeleted == false)
                                on sc.ApartmentId equals a.Id
                            join b in _unitOfWork.BuildingRepository.GetQuery(x => x.IsDeleted == false)
                                on a.BuildingId equals b.Id
                            group new { sc, s } by new { sc.ApartmentId, a.RoomNumber, b.Name } into apartmentGroup
                            select new ApartmentServiceSummaryDto
                            {
                                ApartmentId = apartmentGroup.Key.ApartmentId ?? new Guid(),
                                RoomNumber = apartmentGroup.Key.RoomNumber,
                                BuildingName = apartmentGroup.Key.Name,
                                TotalServices = apartmentGroup.Sum(x => x.s.Unit == "m2" ? 1 : x.sc.Quantity ?? 0)
                            };

                // Calculate total records before pagination
                var totalRecords = await query.CountAsync();

                // Apply pagination
                var data = await query
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();

                // Wrap the result in PaginatedResponseDto
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


        //public async Task<ResponseData<List<ServiceDetailDto>>> GetServiceDetailsByApartmentId(ServiceDetailRequestDto request)
        //{
        //    try
        //    {
        //        var query = from sc in _unitOfWork.ServiceContractRepository.GetQuery(x => x.ApartmentId == request.ApartmentId && x.IsActive && (x.IsDeleted == false))
        //                    join s in _unitOfWork.ServiceRepository.GetQuery(x => (x.IsDeleted == false))
        //                        on sc.ServiceId equals s.Id
        //                    join a in _unitOfWork.ApartmentRepository.GetQuery(x => (x.IsDeleted == false))
        //                        on sc.ApartmentId equals a.Id
        //                    join at in _unitOfWork.ApartmentTypeRepository.GetQuery(x => x.IsActive && (x.IsDeleted == false))
        //                        on a.ApartmentTypeId equals at.Id
        //                    join ps in _unitOfWork.PackageServiceRepository.GetQuery(x => x.IsActive && (x.IsDeleted == false))
        //                        on sc.PackageServiceId equals ps.Id into psJoin
        //                    from ps in psJoin.DefaultIfEmpty()
        //                    select new
        //                    {
        //                        ServiceName = s.ServiceName,
        //                        Description = s.Description,
        //                        QuantityOrArea = s.Unit == "m2" ? $"{at.LandArea} m2" : $"x{sc.Quantity}",
        //                        UnitPrice = s.UnitPrice,
        //                        StartDate = sc.StartDate ?? DateTime.Now,
        //                        EndDate = sc.EndDate ?? DateTime.Now,
        //                        Discount = ps.Discount ?? 0,
        //                        Unit = s.Unit,
        //                        LandArea = at.LandArea,
        //                        Quantity = sc.Quantity
        //                    };

        //        // Áp dụng bộ lọc theo loại dịch vụ nếu có
        //        if (!string.IsNullOrEmpty(request.ServiceType))
        //        {
        //            query = query.Where(x => x.ServiceName.Contains(request.ServiceType));
        //        }

        //        var result = await query.ToListAsync();

        //        // Tính toán TotalPrice sau khi dữ liệu được tải
        //        var data = result.Select(item => new ServiceDetailDto
        //        {
        //            ServiceName = item.ServiceName,
        //            Description = item.Description,
        //            QuantityOrArea = item.QuantityOrArea,
        //            UnitPrice = item.UnitPrice,
        //            StartDate = item.StartDate,
        //            EndDate = item.EndDate,
        //            TotalPrice = CalculateTotalPrice(item.UnitPrice, item.StartDate, item.EndDate, item.Discount, item.Unit, item.LandArea, item.Quantity)
        //        }).ToList();

        //        return new ResponseData<List<ServiceDetailDto>>
        //        {
        //            Success = true,
        //            Message = "Successfully retrieved service details.",
        //            Data = data,
        //            Code = (int)ErrorCodeAPI.OK
        //        };
        //    }
        //    catch (Exception ex)
        //    {
        //        return new ResponseData<List<ServiceDetailDto>>
        //        {
        //            Success = false,
        //            Message = ex.Message,
        //            Code = (int)ErrorCodeAPI.SystemIsError
        //        };
        //    }
        //}

        //public async Task<ResponseData<List<ServiceDetailDto>>> GetServiceDetailsByApartmentId(ServiceDetailRequestDto request)
        //{
        //    try
        //    {
        //        var query = from sc in _unitOfWork.ServiceContractRepository.GetQuery(x => x.ApartmentId == request.ApartmentId && x.IsActive && (x.IsDeleted == false))
        //                    join s in _unitOfWork.ServiceRepository.GetQuery(x => (x.IsDeleted == false))
        //                        on sc.ServiceId equals s.Id
        //                    join a in _unitOfWork.ApartmentRepository.GetQuery(x => (x.IsDeleted == false))
        //                        on sc.ApartmentId equals a.Id
        //                    join at in _unitOfWork.ApartmentTypeRepository.GetQuery(x => x.IsActive && (x.IsDeleted == false))
        //                        on a.ApartmentTypeId equals at.Id
        //                    join ps in _unitOfWork.PackageServiceRepository.GetQuery(x => x.IsActive && (x.IsDeleted == false))
        //                        on sc.PackageServiceId equals ps.Id into psJoin
        //                    from ps in psJoin.DefaultIfEmpty()
        //                    select new
        //                    {
        //                        ServiceName = s.ServiceName,
        //                        Description = s.Description,
        //                        QuantityOrArea = s.Unit == "m2" ? $"{at.LandArea} m2" : $"x{sc.Quantity}",
        //                        UnitPrice = s.UnitPrice,
        //                        StartDate = sc.StartDate ?? DateTime.Now,
        //                        EndDate = sc.EndDate ?? DateTime.Now,
        //                        Discount = ps.Discount ?? 0,
        //                        Unit = s.Unit,
        //                        LandArea = at.LandArea,
        //                        Quantity = sc.Quantity
        //                    };

        //        // Filter by service type if provided
        //        if (!string.IsNullOrEmpty(request.ServiceType))
        //        {
        //            query = query.Where(x => x.ServiceName.Contains(request.ServiceType));
        //        }

        //        // Sort by StartDate descending
        //        query = query.OrderByDescending(x => x.StartDate);

        //        var result = await query.ToListAsync();

        //        // Map and filter data
        //        var data = result
        //            .Select(item =>
        //            {
        //                var totalPrice = CalculateTotalPrice(item.UnitPrice, item.StartDate, item.EndDate, item.Discount, item.Unit, item.LandArea, item.Quantity);
        //                return new ServiceDetailDto
        //                {
        //                    ServiceName = item.ServiceName,
        //                    Description = item.Description,
        //                    QuantityOrArea = item.QuantityOrArea,
        //                    UnitPrice = item.UnitPrice == 0 ? null : item.UnitPrice, // Set to null if 0
        //                    StartDate = item.StartDate,
        //                    EndDate = item.EndDate,
        //                    TotalPrice = totalPrice == 0 ? null : totalPrice // Set to null if 0
        //                };
        //            })
        //            .Where(x => x.UnitPrice != null || x.TotalPrice != null) // Exclude items with both prices null
        //            .ToList();

        //        return new ResponseData<List<ServiceDetailDto>>
        //        {
        //            Success = true,
        //            Message = "Successfully retrieved service details.",
        //            Data = data,
        //            Code = (int)ErrorCodeAPI.OK
        //        };
        //    }
        //    catch (Exception ex)
        //    {
        //        return new ResponseData<List<ServiceDetailDto>>
        //        {
        //            Success = false,
        //            Message = ex.Message,
        //            Code = (int)ErrorCodeAPI.SystemIsError
        //        };
        //    }
        //}

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
                            join ps in _unitOfWork.PackageServiceRepository.GetQuery(x => x.IsActive && (x.IsDeleted == false))
                                on sc.PackageServiceId equals ps.Id into psJoin
                            from ps in psJoin.DefaultIfEmpty()
                            select new
                            {
                                ServiceName = s.ServiceName,
                                Description = s.Description,
                                QuantityOrArea = s.Unit == "m2" ? $"{at.LandArea} m2" : $"x{sc.Quantity}",
                                UnitPrice = s.UnitPrice,
                                StartDate = sc.StartDate ?? DateTime.Now,
                                EndDate = sc.EndDate ?? DateTime.Now,
                                Discount = ps.Discount ?? 0,
                                Unit = s.Unit,
                                LandArea = at.LandArea,
                                Quantity = sc.Quantity
                            };

                // Filter by service type if provided
                if (!string.IsNullOrEmpty(request.ServiceType))
                {
                    query = query.Where(x => x.ServiceName.Contains(request.ServiceType));
                }

                // Filter by StartDate range
                if (request.StartDateFrom.HasValue)
                {
                    query = query.Where(x => x.StartDate >= request.StartDateFrom.Value);
                }
                if (request.StartDateTo.HasValue)
                {
                    query = query.Where(x => x.StartDate <= request.StartDateTo.Value);
                }

                // Sort by StartDate descending
                query = query.OrderByDescending(x => x.StartDate);

                var result = await query.ToListAsync();

                // Map and filter data
                var data = result
                    .Select(item =>
                    {
                        var totalPrice = CalculateTotalPrice(item.UnitPrice, item.StartDate, item.EndDate, item.Discount, item.Unit, item.LandArea, item.Quantity);
                        return new ServiceDetailDto
                        {
                            ServiceName = item.ServiceName,
                            Description = item.Description,
                            QuantityOrArea = item.QuantityOrArea,
                            UnitPrice = item.UnitPrice == 0 ? null : item.UnitPrice, // Set to null if 0
                            StartDate = item.StartDate,
                            EndDate = item.EndDate,
                            TotalPrice = totalPrice == 0 ? null : totalPrice // Set to null if 0
                        };
                    })
                    .Where(x => x.UnitPrice != null || x.TotalPrice != null) // Exclude items with both prices null
                    .ToList();

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



        // Helper method to calculate TotalPrice
        private static decimal CalculateTotalPrice(decimal unitPrice, DateTime startDate, DateTime endDate, decimal discount, string unit, decimal landArea, int? quantity)
        {
            if (unit == "m2")
            {
                // Tính số tháng giữa StartDate và EndDate
                int months = ((endDate.Year - startDate.Year) * 12) + endDate.Month - startDate.Month + 1;

                // Tính giá dựa trên số tháng và diện tích LandArea
                //decimal baseAmount = unitPrice * landArea * months;

                // Tính giá dựa trên số tháng
                decimal baseAmount = unitPrice * months;



                // Áp dụng chiết khấu cho từng đơn vị
                //return baseAmount * (1 - discount / 100);
                return baseAmount;
            }
            else
            {
                // Tính số ngày giữa StartDate và EndDate
                int days = (endDate - startDate).Days;

                // Tính giá cơ bản cho từng đơn vị dựa trên Quantity và số ngày
                decimal baseAmount = unitPrice * (quantity ?? 0) * days;

                // Áp dụng chiết khấu cho từng đơn vị
                return baseAmount * (1 - discount / 100);
            }
        }




        //public async Task<ResponseData<PaginatedResponseDto<UnpaidServiceSummaryDto>>> GetUnpaidServiceSummaryByUserId(Guid userId, int pageSize, int pageNumber)
        //{
        //    try
        //    {
        //        var currentMonth = DateTime.Now.Month;
        //        var currentYear = DateTime.Now.Year;

        //        var query = from inv in _unitOfWork.InvoiceRepository.GetQuery(x => x.PaidStatus == false && x.IsDeleted == false)
        //                    join sc in _unitOfWork.ServiceContractRepository.GetQuery(x => x.IsActive && x.IsDeleted == false)
        //                        on inv.ServiceContractId equals sc.Id
        //                    join o in _unitOfWork.OwnerShipRepository.GetQuery(x => x.ResidentId == userId && x.IsDeleted == false)
        //                        on sc.ApartmentId equals o.ApartmentId
        //                    join a in _unitOfWork.ApartmentRepository.GetQuery(x => x.IsDeleted == false)
        //                        on sc.ApartmentId equals a.Id
        //                    where inv.IssueDate.HasValue && inv.IssueDate.Value.Month == currentMonth && inv.IssueDate.Value.Year == currentYear
        //                    group new { inv, sc } by new { a.Id, a.RoomNumber } into apartmentGroup
        //                    select new UnpaidServiceSummaryDto
        //                    {
        //                        ApartmentId = apartmentGroup.Key.Id, // Trả về ApartmentId
        //                        RoomNumber = apartmentGroup.Key.RoomNumber,
        //                        TotalServices = apartmentGroup.Sum(x => x.sc.Quantity ?? 1), // Tổng số lượng từ cột Quantity của bảng ServiceContract
        //                        Month = currentMonth,
        //                        PaymentStatus = "Chưa thanh toán"
        //                    };



        //        // Tính tổng số bản ghi trước khi phân trang
        //        var totalRecords = await query.CountAsync();

        //        // Áp dụng phân trang
        //        var data = await query
        //            .Skip((pageNumber - 1) * pageSize)
        //            .Take(pageSize)
        //            .ToListAsync();

        //        // Đóng gói kết quả vào PaginatedResponseDto
        //        var response = new PaginatedResponseDto<UnpaidServiceSummaryDto>
        //        {
        //            TotalRecords = totalRecords,
        //            Data = data
        //        };

        //        return new ResponseData<PaginatedResponseDto<UnpaidServiceSummaryDto>>
        //        {
        //            Success = true,
        //            Message = "Successfully retrieved unpaid service summary.",
        //            Data = response,
        //            Code = (int)ErrorCodeAPI.OK
        //        };
        //    }
        //    catch (Exception ex)
        //    {
        //        return new ResponseData<PaginatedResponseDto<UnpaidServiceSummaryDto>>
        //        {
        //            Success = false,
        //            Message = ex.Message,
        //            Code = (int)ErrorCodeAPI.SystemIsError
        //        };
        //    }
        //}

        public async Task<ResponseData<PaginatedResponseDto<UnpaidServiceSummaryDto>>> GetServiceSummaryByUserId(
      Guid userId, ServiceSummaryRequestDto request)
        {
            try
            {
                var query = from inv in _unitOfWork.InvoiceRepository.GetQuery(x => x.IsDeleted == false)
                            join sc in _unitOfWork.ServiceContractRepository.GetQuery(x => x.IsActive && x.IsDeleted == false)
                                on inv.ServiceContractId equals sc.Id
                            join o in _unitOfWork.OwnerShipRepository.GetQuery(x => x.ResidentId == userId && x.IsDeleted == false)
                                on sc.ApartmentId equals o.ApartmentId
                            join a in _unitOfWork.ApartmentRepository.GetQuery(x => x.IsDeleted == false)
                                on sc.ApartmentId equals a.Id
                            join b in _unitOfWork.BuildingRepository.GetQuery(x => x.IsDeleted == false)
                                on a.BuildingId equals b.Id
                            select new UnpaidServiceSummaryDto
                            {
                                ApartmentId = a.Id,
                                RoomNumber = a.RoomNumber,
                                BuildingId = b.Id,
                                BuildingName = b.Name,
                                TotalServices = sc.Quantity ?? 1,
                                Month = inv.IssueDate.HasValue ? inv.IssueDate.Value.Month : 0,
                                PaymentStatus = inv.PaidStatus ? "Đã thanh toán" : "Chưa thanh toán"
                            };

                // Apply filters from request
                if (request.BuildingIdFilter.HasValue)
                {
                    query = query.Where(x => x.BuildingId == request.BuildingIdFilter.Value);
                }
                if (request.ApartmentIdFilter.HasValue)
                {
                    query = query.Where(x => x.ApartmentId == request.ApartmentIdFilter.Value);
                }
                if (!string.IsNullOrEmpty(request.PaymentStatusFilter))
                {
                    query = query.Where(x => x.PaymentStatus == request.PaymentStatusFilter);
                }
                if (request.MonthFilter.HasValue)
                {
                    query = query.Where(x => x.Month == request.MonthFilter.Value);
                }

                // Sort by month
                query = query.OrderByDescending(x => x.Month);

                // Calculate total records before pagination
                var totalRecords = await query.CountAsync();

                // Apply pagination
                var data = await query
                    .Skip((request.PageNumber - 1) * request.PageSize)
                    .Take(request.PageSize)
                    .ToListAsync();

                // Wrap the result in PaginatedResponseDto
                var response = new PaginatedResponseDto<UnpaidServiceSummaryDto>
                {
                    TotalRecords = totalRecords,
                    Data = data
                };

                return new ResponseData<PaginatedResponseDto<UnpaidServiceSummaryDto>>
                {
                    Success = true,
                    Message = "Successfully retrieved service summary.",
                    Data = response,
                    Code = (int)ErrorCodeAPI.OK
                };
            }
            catch (Exception ex)
            {
                return new ResponseData<PaginatedResponseDto<UnpaidServiceSummaryDto>>
                {
                    Success = false,
                    Message = ex.Message,
                    Code = (int)ErrorCodeAPI.SystemIsError
                };
            }
        }




        public async Task<ResponseData<UnpaidServiceDetailResponseDto>> GetUnpaidServiceDetailsByApartmentId(UnpaidServiceDetailRequestDto request)
        {
            try
            {
                var currentMonth = DateTime.Now.Month;
                var currentYear = DateTime.Now.Year;

                //var query = from inv in _unitOfWork.InvoiceRepository.GetQuery(x => x.PaidStatus == false && (x.IsDeleted == false))   
                var query = from inv in _unitOfWork.InvoiceRepository.GetQuery(x => (x.IsDeleted == false))
                            join sc in _unitOfWork.ServiceContractRepository.GetQuery(x => x.IsActive && (x.IsDeleted == false) && x.ApartmentId == request.ApartmentId)
                                on inv.ServiceContractId equals sc.Id
                            join s in _unitOfWork.ServiceRepository.GetQuery(x => (x.IsDeleted == false))
                                on sc.ServiceId equals s.Id
                            join a in _unitOfWork.ApartmentRepository.GetQuery(x => (x.IsDeleted == false))
                                on sc.ApartmentId equals a.Id
                            join at in _unitOfWork.ApartmentTypeRepository.GetQuery(x => x.IsActive && (x.IsDeleted == false))
                                on a.ApartmentTypeId equals at.Id
                            join ps in _unitOfWork.PackageServiceRepository.GetQuery(x => x.IsActive && (x.IsDeleted == false))
                                on sc.PackageServiceId equals ps.Id into psJoin
                            from ps in psJoin.DefaultIfEmpty()
                            where inv.IssueDate.HasValue && inv.IssueDate.Value.Month == currentMonth && inv.IssueDate.Value.Year == currentYear
                            select new
                            {
                                InvoiceId = inv.Id,
                                ServiceName = s.ServiceName,
                                Description = s.Description,
                                QuantityOrArea = s.Unit == "m2" ? $"{at.LandArea} m2" : $"x{sc.Quantity}",
                                UnitPrice = s.UnitPrice,
                                Discount = ps.Discount ?? 0,
                                Unit = s.Unit,
                                LandArea = at.LandArea,
                                Quantity = sc.Quantity,
                                StartDate = sc.StartDate ?? DateTime.Now,
                                EndDate = sc.EndDate ?? DateTime.Now
                            };

                // Áp dụng bộ lọc theo loại dịch vụ nếu có
                if (!string.IsNullOrEmpty(request.ServiceType))
                {
                    query = query.Where(x => x.ServiceName.Contains(request.ServiceType));
                }

                var result = await query.ToListAsync();

                // Tính toán TotalPrice sau khi dữ liệu được tải
                var services = result.Select(item => new UnpaidServiceDetailDto
                {
                    InvoiceId = item.InvoiceId,
                    ServiceName = item.ServiceName,
                    Description = item.Description,
                    QuantityOrArea = item.QuantityOrArea,
                    UnitPrice = item.UnitPrice,
                    TotalPrice = CalculateTotalPrice(item.UnitPrice, item.StartDate, item.EndDate, item.Discount, item.Unit, item.LandArea, item.Quantity)
                }).ToList();

                // Tính tổng giá của tất cả dịch vụ
                var totalAmount = services.Sum(x => x.TotalPrice);

                // Đóng gói kết quả vào UnpaidServiceDetailResponseDto
                var response = new UnpaidServiceDetailResponseDto
                {
                    Services = services,
                    TotalAmount = totalAmount
                };

                return new ResponseData<UnpaidServiceDetailResponseDto>
                {
                    Success = true,
                    Message = "Successfully retrieved unpaid service details.",
                    Data = response,
                    Code = (int)ErrorCodeAPI.OK
                };
            }
            catch (Exception ex)
            {
                return new ResponseData<UnpaidServiceDetailResponseDto>
                {
                    Success = false,
                    Message = ex.Message,
                    Code = (int)ErrorCodeAPI.SystemIsError
                };
            }
        }
    }
}
