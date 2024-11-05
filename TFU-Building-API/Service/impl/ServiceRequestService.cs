using Core.Enums;
using Core.Model;
using TFU_Building_API.Core.Helper;
using TFU_Building_API.Core.Infrastructure;
using TFU_Building_API.Dto;
using TFU_Building_API.Service;

public class ServiceRequestService : IServiceRequestService
{
    private readonly IUnitOfWork _unitOfWork;

    public ServiceRequestService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<ResponseData<PaginatedList<ServiceRequestDto>>> GetServiceRequestsAsync(ServiceRequestSearchDto searchDto)
    {
        try
        {
            // Set up query to join the necessary tables and filter as needed
            var query = from serviceContract in _unitOfWork.ServiceContractRepository.GetQuery(sc => sc.IsDeleted == false)
                        join apartment in _unitOfWork.ApartmentRepository.GetQuery(a => a.IsDeleted == false) on serviceContract.ApartmentId equals apartment.Id
                        join building in _unitOfWork.BuildingRepository.GetQuery(b => b.IsDeleted == false) on apartment.BuildingId equals building.Id
                        join service in _unitOfWork.ServiceRepository.GetQuery(s => s.IsDeleted == false) on serviceContract.ServiceId equals service.Id
                        where (string.IsNullOrEmpty(searchDto.ServiceName) || service.ServiceName.Contains(searchDto.ServiceName)) &&
                              (searchDto.BuildingId == null || building.Id == searchDto.BuildingId) &&
                              (searchDto.Status == null || serviceContract.Status == searchDto.Status)
                        select new ServiceRequestDto
                        {
                            ServiceType = service.ServiceName,
                            Date = serviceContract.StartDate ?? DateTime.Now,
                            Building = building.Name,
                            Status = serviceContract.Status == ServiceContractStatus.Pending ? "Đang xử lý" :
                                     serviceContract.Status == ServiceContractStatus.Approved ? "Đồng ý" :
                                     serviceContract.Status == ServiceContractStatus.Rejected ? "Reject" : "Unknown",
                            ServiceContractId = serviceContract.Id
                        };

            // Execute query and use synchronous methods
            var itemsList = query.ToList(); // Get the full result set to perform synchronous operations
            int totalRecords = itemsList.Count;

            // Apply pagination manually on the in-memory collection
            var items = itemsList
                .Skip((searchDto.PageNumber - 1) * searchDto.PageSize)
                .Take(searchDto.PageSize)
                .ToList();

            // Build paginated result
            var paginatedList = new PaginatedList<ServiceRequestDto>
            {
                Items = items,
                PageNumber = searchDto.PageNumber,
                PageSize = searchDto.PageSize,
                TotalRecords = totalRecords
            };

            return new ResponseData<PaginatedList<ServiceRequestDto>>
            {
                Success = true,
                Message = "Service requests retrieved successfully.",
                Data = paginatedList,
                Code = (int)ErrorCodeAPI.OK
            };
        }
        catch (Exception ex)
        {
            return new ResponseData<PaginatedList<ServiceRequestDto>>
            {
                Success = false,
                Message = ex.Message,
                Data = null,
                Code = (int)ErrorCodeAPI.SystemIsError
            };
        }
    }

}
