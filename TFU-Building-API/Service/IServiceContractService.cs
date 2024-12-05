using Core.Model;
using TFU_Building_API.Dto;

namespace TFU_Building_API.Service
{
    public interface IServiceContractService
    {
        Task<ResponseData<string>> AddServiceContract(CreateServiceContractRequestDto request);

        Task<ResponseData<List<ServiceContractDetailDto>>> GetServiceContractDetails(Guid apartmentId);
        Task<ResponseData<PaginatedResponseDto<ServiceContractDetailDto>>> GetServiceContractDetails(ServiceContractDetailRequestDto request);
        Task<ResponseData<List<AddVehicleServiceResponseDto>>> AddVehicleServiceAsync(AddVehicleServiceRequestDto request);
        Task<ResponseData<List<AddRepairReportServiceResponseDto>>> AddRepairReportServiceAsync(AddRepairReportServiceRequestDto request);

        Task<ResponseData<VehicleServiceDetailDto>> GetVehicleServiceDetailAsync(Guid serviceContractId);
        Task<ResponseData<RepairReportServiceDetailDto>> GetRepairReportServiceDetailAsync(Guid serviceContractId);

        Task<ResponseData<AddVehicleServiceResponseDto>> UpdateVehicleServiceRequestAsync(UpdateVehicleServiceRequestDto request);
        Task<ResponseData<AddRepairReportServiceResponseDto>> UpdateRepairReportServiceRequestAsync(UpdateRepairReportServiceRequestDto request);


        Task<ResponseData<string>> AddServiceContractThirdPartyAsync(AddServiceContractThirdPartyRequestDto request);

        Task<ResponseData<string>> AddMonthlyFixedServiceContractsAsync(Guid userId);
    }
}
