using Core.Model;
using TFU_Building_API.Dto;

namespace TFU_Building_API.Service
{
    public interface IServiceContractService
    {
        Task<ResponseData<string>> AddServiceContract(CreateServiceContractRequestDto request);

        Task<ResponseData<List<ServiceContractDetailDto>>> GetServiceContractDetails(Guid apartmentId);
        Task<ResponseData<PaginatedResponseDto<ServiceContractDetailDto>>> GetServiceContractDetails(ServiceContractDetailRequestDto request);
    }
}
