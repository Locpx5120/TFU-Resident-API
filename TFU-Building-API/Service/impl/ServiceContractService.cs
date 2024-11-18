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
                                ServiceContractId = sc.Id,
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
                    ServiceContractId = item.ServiceContractId,
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
                // Building the query with additional filters for service name, building, and service type
                var query = from sc in _unitOfWork.ServiceContractRepository.GetQuery(x => (x.IsDeleted == false))
                            join s in _unitOfWork.ServiceRepository.GetQuery(x => (x.IsDeleted == false))
                                on sc.ServiceId equals s.Id
                            join a in _unitOfWork.ApartmentRepository.GetQuery(x => (x.IsDeleted == false))
                                on sc.ApartmentId equals a.Id
                            join b in _unitOfWork.BuildingRepository.GetQuery(x => (x.IsDeleted == false))
                                on a.BuildingId equals b.Id
                            join at in _unitOfWork.ApartmentTypeRepository.GetQuery(x => x.IsActive && (x.IsDeleted == false))
                                on a.ApartmentTypeId equals at.Id
                            where (string.IsNullOrEmpty(request.ServiceName) || s.ServiceName.Contains(request.ServiceName)) &&
                                  (request.BuildingId == null || b.Id == request.BuildingId) &&
                                  (request.ServiceTypeId == null || s.ServiceCategoryID == request.ServiceTypeId)
                            select new
                            {
                                ServiceContractId = sc.Id,
                                Apartment = a.RoomNumber,
                                Building = b.Name,
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

                // Get the total record count before pagination
                var totalRecords = await query.CountAsync();

                // Apply pagination
                var pagedData = await query
                    .Skip((request.PageNumber - 1) * request.PageSize)
                    .Take(request.PageSize)
                    .ToListAsync();

                // Process the data outside of the query for additional formatting
                var result = pagedData.Select(item => new ServiceContractDetailDto
                {
                    ServiceContractId = item.ServiceContractId,
                    Apartment = item.Apartment,
                    Building = item.Building,
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
                        IsDeleted = true,
                        InsertedAt = DateTime.Now,
                        UpdatedAt = DateTime.Now,
                        IsActive = false
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
                        Status = ServiceContractStatus.Pending, // Assuming 1 means "Active"
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

        public async Task<ResponseData<VehicleServiceDetailDto>> GetVehicleServiceDetailAsync(Guid serviceContractId)
        {
            try
            {
                var serviceDetail = await (from sc in _unitOfWork.ServiceContractRepository.GetQuery(sc => sc.Id == serviceContractId && sc.IsDeleted == false)
                                           join a in _unitOfWork.ApartmentRepository.GetQuery(a => a.IsDeleted == false) on sc.ApartmentId equals a.Id
                                           join b in _unitOfWork.BuildingRepository.GetQuery(b => b.IsDeleted == false) on a.BuildingId equals b.Id
                                           join s in _unitOfWork.ServiceRepository.GetQuery(s => s.IsDeleted == false) on sc.ServiceId equals s.Id
                                           join ps in _unitOfWork.PackageServiceRepository.GetQuery(ps => ps.IsDeleted == false) on sc.PackageServiceId equals ps.Id
                                           join v in _unitOfWork.VehicleRepository.GetQuery(v => v.IsDeleted == false && v.IsActive) on sc.VehicleId equals v.Id
                                           select new VehicleServiceDetailDto
                                           {
                                               BuildingName = b.Name,
                                               ApartmentNumber = a.RoomNumber,
                                               ServiceName = s.ServiceName,
                                               Package = $"{ps.DurationInMonth} tháng",
                                               VehicleType = v.VehicleType,
                                               LicensePlate = v.LicensePlate,
                                               StartDate = sc.StartDate??DateTime.Now,
                                               EndDate = sc.EndDate??DateTime.Now,
                                               Note = sc.Note
                                           }).FirstOrDefaultAsync();

                if (serviceDetail == null)
                {
                    return new ResponseData<VehicleServiceDetailDto>
                    {
                        Success = false,
                        Message = "Service contract not found",
                        Code = (int)ErrorCodeAPI.NotFound
                    };
                }

                return new ResponseData<VehicleServiceDetailDto>
                {
                    Success = true,
                    Message = "Vehicle service details retrieved successfully",
                    Data = serviceDetail,
                    Code = (int)ErrorCodeAPI.OK
                };
            }
            catch (Exception ex)
            {
                return new ResponseData<VehicleServiceDetailDto>
                {
                    Success = false,
                    Message = ex.Message,
                    Code = (int)ErrorCodeAPI.SystemIsError
                };
            }
        }

       

        public async Task<ResponseData<AddVehicleServiceResponseDto>> UpdateVehicleServiceRequestAsync(UpdateVehicleServiceRequestDto request)
        {
            try
            {
                // Retrieve the service contract based on the provided ID
                var serviceContract = await _unitOfWork.ServiceContractRepository
                    .GetQuery(sc => sc.Id == request.ServiceContractId && sc.IsDeleted == false)
                    .FirstOrDefaultAsync();

                if (serviceContract == null)
                {
                    return new ResponseData<AddVehicleServiceResponseDto>
                    {
                        Success = false,
                        Message = "Service contract not found.",
                        Data = new AddVehicleServiceResponseDto { Success = false, Message = "Service contract not found." },
                        Code = (int)ErrorCodeAPI.NotFound
                    };
                }

                // Update the service contract status and note
                serviceContract.Status = request.Status;
                serviceContract.Note = request.Note;
                serviceContract.UpdatedAt = DateTime.Now;

                // If Status = 1, determine which field to update
                if (request.Status == ServiceContractStatus.Approved)
                {
                    if (serviceContract.VehicleId.HasValue)
                    {
                        // Update the corresponding record in Vehicles
                        var vehicle = await _unitOfWork.VehicleRepository
                            .GetByIdAsync(serviceContract.VehicleId.Value);

                        if (vehicle != null)
                        {
                            vehicle.IsActive = true;
                            vehicle.IsDeleted = false;
                            vehicle.UpdatedAt = DateTime.Now;
                            _unitOfWork.VehicleRepository.Update(vehicle);
                        }
                    }
                    else if (serviceContract.LivingId.HasValue)
                    {
                        // Update the corresponding record in Livings
                        var living = await _unitOfWork.LivingRepository
                            .GetByIdAsync(serviceContract.LivingId ?? new Guid());
                            

                        if (living != null)
                        {
                            living.IsActive = true;
                            living.IsDeleted = false;
                            living.UpdatedAt = DateTime.Now;
                            _unitOfWork.LivingRepository.Update(living);
                        }
                    }
                }

                // Save changes
                _unitOfWork.ServiceContractRepository.Update(serviceContract);
                await _unitOfWork.SaveChangesAsync();

                return new ResponseData<AddVehicleServiceResponseDto>
                {
                    Success = true,
                    Message = "Vehicle service request updated successfully.",
                    Data = new AddVehicleServiceResponseDto { Success = true, Message = "Service request updated successfully." },
                    Code = (int)ErrorCodeAPI.OK
                };
            }
            catch (Exception ex)
            {
                return new ResponseData<AddVehicleServiceResponseDto>
                {
                    Success = false,
                    Message = ex.Message,
                    Data = new AddVehicleServiceResponseDto { Success = false, Message = ex.Message },
                    Code = (int)ErrorCodeAPI.SystemIsError
                };
            }
        }

    }
}
