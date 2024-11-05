using System;
using System.Threading.Tasks;
using TFU_Building_API.Core.Infrastructure;
using TFU_Building_API.Dto;
using Core.Model;
using BuildingModels;
using Core.Enums;
using Microsoft.EntityFrameworkCore;
using TFU_Building_API.Core.Helper;
using TFU_Building_API.Core.Handler;

namespace TFU_Building_API.Service.impl
{
    public class ServiceContractService : BaseHandler, IServiceContractService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ServiceContractService(IUnitOfWork UnitOfWork, IHttpContextAccessor HttpContextAccessor) : base(UnitOfWork, HttpContextAccessor)
        {
            _unitOfWork = UnitOfWork;
        }

        public async Task<ResponseData<string>> AddServiceContract(CreateServiceContractRequestDto request)
        {
            try
            {
                var packageService = await _unitOfWork.PackageServiceRepository.GetQuery(x => x.Id == request.PackageServiceId && (x.IsDeleted == false))
                                                                                .FirstOrDefaultAsync();

                if (packageService == null)
                {
                    return new ResponseData<string>
                    {
                        Success = false,
                        Message = "Package service not found.",
                        Code = (int)ErrorCodeAPI.NotFound
                    };
                }

                DateTime endDate = request.StartDate.AddMonths(packageService.DurationInMonth);

                var serviceContract = new ServiceContract
                {
                    Id = Guid.NewGuid(),
                    StartDate = request.StartDate,
                    EndDate = endDate,
                    Status = ServiceContractStatus.Pending,
                    Quantity = request.Quantity,
                    ApartmentId = request.ApartmentId,
                    ServiceId = request.ServiceId,
                    IsDeleted = false,
                    InsertedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now,
                    IsActive = true,
                    Note = request.Note,
                    PackageServiceId = request.PackageServiceId
                };

                _unitOfWork.ServiceContractRepository.Add(serviceContract);
                await _unitOfWork.SaveChangesAsync();

                return new ResponseData<string>
                {
                    Success = true,
                    Message = "Service contract created successfully.",
                    Code = (int)ErrorCodeAPI.OK
                };
            }
            catch (Exception ex)
            {
                return new ResponseData<string>
                {
                    Success = false,
                    Message = ex.Message,
                    Code = (int)ErrorCodeAPI.SystemIsError
                };
            }
        }

        public async Task<ResponseData<List<ServiceContractDetailDto>>> GetServiceContractDetails(Guid apartmentId)
        {
            try
            {
                // Lấy dữ liệu thô từ database
                var query = from sc in _unitOfWork.ServiceContractRepository.GetQuery(x => x.ApartmentId == apartmentId && (x.IsDeleted == false))
                            join s in _unitOfWork.ServiceRepository.GetQuery(x => (x.IsDeleted == false))
                                on sc.ServiceId equals s.Id
                            join ps in _unitOfWork.PackageServiceRepository.GetQuery(x => (x.IsDeleted == false))
                                on sc.PackageServiceId equals ps.Id into psJoin
                            from ps in psJoin.DefaultIfEmpty()
                            select new
                            {
                                ServiceName = s.ServiceName,
                                Purpose = s.Description,
                                CreatedDate = sc.StartDate ?? DateTime.Now,
                                QuantityOrArea = s.Unit == "m2" ? $"{sc.Apartment.ApartmentType.LandArea} m2" : $"x{sc.Quantity}",
                                UnitPrice = s.UnitPrice,
                                Status = sc.Status,
                                Note = sc.Note,
                                ProcessedDate = sc.Status == ServiceContractStatus.Approved || sc.Status == ServiceContractStatus.Rejected
                                    ? sc.UpdatedAt : (DateTime?)null
                            };

                var rawData = await query.ToListAsync();

                // Áp dụng xử lý Status bên ngoài truy vấn
                var result = rawData.Select(item => new ServiceContractDetailDto
                {
                    ServiceName = item.ServiceName,
                    Purpose = item.Purpose,
                    CreatedDate = item.CreatedDate,
                    QuantityOrArea = item.QuantityOrArea,
                    UnitPrice = item.UnitPrice,
                    Status = GetStatusDescription(item.Status),
                    Note = item.Note,
                    ProcessedDate = item.ProcessedDate
                }).ToList();

                return new ResponseData<List<ServiceContractDetailDto>>
                {
                    Success = true,
                    Message = "Successfully retrieved service contract details.",
                    Data = result,
                    Code = (int)ErrorCodeAPI.OK
                };
            }
            catch (Exception ex)
            {
                return new ResponseData<List<ServiceContractDetailDto>>
                {
                    Success = false,
                    Message = ex.Message,
                    Code = (int)ErrorCodeAPI.SystemIsError
                };
            }
        }

        //public async Task<ResponseData<List<ServiceContractDetailDto>>> GetServiceContractDetails()
        //{
        //    try
        //    {
        //        var query = from sc in _unitOfWork.ServiceContractRepository.GetQuery(x => (x.IsDeleted == false))
        //                    join s in _unitOfWork.ServiceRepository.GetQuery(x => (x.IsDeleted == false))
        //                        on sc.ServiceId equals s.Id
        //                    join a in _unitOfWork.ApartmentRepository.GetQuery(x => (x.IsDeleted == false))
        //                        on sc.ApartmentId equals a.Id
        //                    join at in _unitOfWork.ApartmentTypeRepository.GetQuery(x => x.IsActive && (x.IsDeleted == false))
        //                        on a.ApartmentTypeId equals at.Id
        //                    select new
        //                    {
        //                        Apartment = a.RoomNumber,
        //                        ServiceName = s.ServiceName,
        //                        Purpose = s.Description,
        //                        CreatedDate = sc.StartDate ?? DateTime.Now,
        //                        QuantityOrArea = s.Unit == "m2" ? $"{at.LandArea} m2" : $"x{sc.Quantity}",
        //                        UnitPrice = s.UnitPrice,
        //                        Status = sc.Status,
        //                        Note = sc.Note,
        //                        ProcessedDate = sc.Status == ServiceContractStatus.Approved || sc.Status == ServiceContractStatus.Rejected
        //                            ? sc.UpdatedAt : (DateTime?)null
        //                    };

        //        var rawData = await query.ToListAsync();

        //        // Áp dụng xử lý Status bên ngoài truy vấn
        //        var result = rawData.Select(item => new ServiceContractDetailDto
        //        {
        //            Apartment = item.Apartment,
        //            ServiceName = item.ServiceName,
        //            Purpose = item.Purpose,
        //            CreatedDate = item.CreatedDate,
        //            QuantityOrArea = item.QuantityOrArea,
        //            UnitPrice = item.UnitPrice,
        //            Status = GetStatusDescription(item.Status),
        //            Note = item.Note,
        //            ProcessedDate = item.ProcessedDate
        //        }).ToList();

        //        return new ResponseData<List<ServiceContractDetailDto>>
        //        {
        //            Success = true,
        //            Message = "Successfully retrieved service contract details.",
        //            Data = result,
        //            Code = (int)ErrorCodeAPI.OK
        //        };
        //    }
        //    catch (Exception ex)
        //    {
        //        return new ResponseData<List<ServiceContractDetailDto>>
        //        {
        //            Success = false,
        //            Message = ex.Message,
        //            Code = (int)ErrorCodeAPI.SystemIsError
        //        };
        //    }
        //}

        // Đổi `GetStatusDescription` thành phương thức static

        public async Task<ResponseData<PaginatedResponseDto<ServiceContractDetailDto>>> GetServiceContractDetails(ServiceContractDetailRequestDto request)
        {
            try
            {
                var query = from sc in _unitOfWork.ServiceContractRepository.GetQuery(x => (x.IsDeleted == false))
                            join s in _unitOfWork.ServiceRepository.GetQuery(x => (x.IsDeleted == false))
                                on sc.ServiceId equals s.Id
                            join a in _unitOfWork.ApartmentRepository.GetQuery(x => (x.IsDeleted == false))
                                on sc.ApartmentId equals a.Id
                            join at in _unitOfWork.ApartmentTypeRepository.GetQuery(x => x.IsActive && (x.IsDeleted == false))
                                on a.ApartmentTypeId equals at.Id
                            select new
                            {
                                Apartment = a.RoomNumber,
                                ServiceName = s.ServiceName,
                                Purpose = s.Description,
                                CreatedDate = sc.StartDate ?? DateTime.Now,
                                QuantityOrArea = s.Unit == "m2" ? $"{at.LandArea} m2" : $"x{sc.Quantity}",
                                UnitPrice = s.UnitPrice,
                                Status = sc.Status,
                                Note = sc.Note,
                                ProcessedDate = sc.Status == ServiceContractStatus.Approved || sc.Status == ServiceContractStatus.Rejected
                                    ? sc.UpdatedAt : (DateTime?)null
                            };

                // Lấy tổng số bản ghi trước khi phân trang
                var totalRecords = await query.CountAsync();

                // Áp dụng phân trang
                var pagedData = await query
                    .Skip((request.PageNumber - 1) * request.PageSize)
                    .Take(request.PageSize)
                    .ToListAsync();

                // Áp dụng xử lý Status bên ngoài truy vấn
                var result = pagedData.Select(item => new ServiceContractDetailDto
                {
                    Apartment = item.Apartment,
                    ServiceName = item.ServiceName,
                    Purpose = item.Purpose,
                    CreatedDate = item.CreatedDate,
                    QuantityOrArea = item.QuantityOrArea,
                    UnitPrice = item.UnitPrice,
                    Status = GetStatusDescription(item.Status),
                    Note = item.Note,
                    ProcessedDate = item.ProcessedDate
                }).ToList();

                var paginatedResponse = new PaginatedResponseDto<ServiceContractDetailDto>
                {
                    TotalRecords = totalRecords,
                    Data = result
                };

                return new ResponseData<PaginatedResponseDto<ServiceContractDetailDto>>
                {
                    Success = true,
                    Message = "Successfully retrieved service contract details.",
                    Data = paginatedResponse,
                    Code = (int)ErrorCodeAPI.OK
                };
            }
            catch (Exception ex)
            {
                return new ResponseData<PaginatedResponseDto<ServiceContractDetailDto>>
                {
                    Success = false,
                    Message = ex.Message,
                    Code = (int)ErrorCodeAPI.SystemIsError
                };
            }
        }

        private static string GetStatusDescription(int? status)
        {
            return status switch
            {
                ServiceContractStatus.Pending => "Processing",
                ServiceContractStatus.Approved => "Approved",
                ServiceContractStatus.Rejected => "Rejected",
                _ => "Unknown"
            };
        }

        public async Task<ResponseData<List<AddVehicleServiceResponseDto>>> AddVehicleServiceAsync(AddVehicleServiceRequestDto request)
        {
            try
            {
                var responseList = new List<AddVehicleServiceResponseDto>();
                var minimumStartDate = DateTime.Now.AddDays(7); // Minimum allowed start date (one week from now)

                foreach (var serviceRequest in request.Services)
                {
                    // Validate StartDate
                    if (serviceRequest.StartDate < minimumStartDate)
                    {
                        responseList.Add(new AddVehicleServiceResponseDto
                        {
                            Success = false,
                            Message = $"Start date for license plate {serviceRequest.LicensePlate} must be at least one week from today."
                        });
                        continue; // Skip to the next service request
                    }

                    // Step 1: Add Vehicle to Vehicles table
                    var vehicle = new Vehicle
                    {
                        Id = Guid.NewGuid(),
                        ResidentId = serviceRequest.ResidentId,
                        VehicleType = serviceRequest.VehicleType,
                        LicensePlate = serviceRequest.LicensePlate,
                        IsDeleted = false,
                        InsertedAt = DateTime.Now,
                        UpdatedAt = DateTime.Now,
                        IsActive = true
                    };
                    _unitOfWork.VehicleRepository.Add(vehicle);

                    // Step 2: Check Package Service
                    var packageService = await _unitOfWork.PackageServiceRepository
                        .GetQuery(p => p.Id == serviceRequest.PackageServiceId && p.IsDeleted == false)
                        .FirstOrDefaultAsync();

                    if (packageService == null)
                    {
                        responseList.Add(new AddVehicleServiceResponseDto
                        {
                            Success = false,
                            Message = $"Invalid package service for license plate {serviceRequest.LicensePlate}."
                        });
                        continue;
                    }

                    // Calculate EndDate based on StartDate and package duration
                    var endDate = serviceRequest.StartDate.AddMonths(packageService.DurationInMonth);

                    // Step 3: Add Service Contract to ServiceContracts table
                    var serviceContract = new ServiceContract
                    {
                        Id = Guid.NewGuid(),
                        StartDate = serviceRequest.StartDate,
                        EndDate = endDate,
                        Status = 1, // Assuming 1 means "Active"
                        Quantity = 1, // Assuming 1 for one vehicle service
                        Note = serviceRequest.Note,
                        ApartmentId = serviceRequest.ApartmentId,
                        ServiceId = serviceRequest.ServiceId,
                        PackageServiceId = serviceRequest.PackageServiceId,
                        VehicleId = vehicle.Id,
                        IsDeleted = false,
                        InsertedAt = DateTime.Now,
                        UpdatedAt = DateTime.Now,
                        IsActive = true
                    };

                    _unitOfWork.ServiceContractRepository.Add(serviceContract);

                    // Add success response for each added service
                    responseList.Add(new AddVehicleServiceResponseDto
                    {
                        Success = true,
                        Message = $"Service added successfully for license plate {serviceRequest.LicensePlate}."
                    });
                }

                // Save all changes at once
                await _unitOfWork.SaveChangesAsync();

                return new ResponseData<List<AddVehicleServiceResponseDto>>
                {
                    Success = true,
                    Message = "Vehicle services added successfully.",
                    Data = responseList,
                    Code = (int)ErrorCodeAPI.OK
                };
            }
            catch (Exception ex)
            {
                return new ResponseData<List<AddVehicleServiceResponseDto>>
                {
                    Success = false,
                    Message = ex.Message,
                    Data = new List<AddVehicleServiceResponseDto>
            {
                new AddVehicleServiceResponseDto
                {
                    Success = false,
                    Message = ex.Message
                }
            },
                    Code = (int)ErrorCodeAPI.SystemIsError
                };
            }
        }


    }
}
