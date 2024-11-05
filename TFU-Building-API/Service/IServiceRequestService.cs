using Core.Model;
using TFU_Building_API.Dto;

namespace TFU_Building_API.Service
{
    public interface IServiceRequestService
    {
        Task<ResponseData<PaginatedList<ServiceRequestDto>>> GetServiceRequestsAsync(ServiceRequestSearchDto searchDto);
    }
}
