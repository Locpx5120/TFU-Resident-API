using TFU_Building_API.Dto;

namespace TFU_Building_API.Core.Dapper.ServiceContract
{
    public interface IServiceContractRepository
    {
        public Task<IEnumerable<ServiceContractDetailDto>> GetServiceContractDetails(Guid apartmentId);
        public Task<IEnumerable<RepairReportServiceDetailDto>> GetRepairReportDetails(Guid serviceContractId);
    }
}
