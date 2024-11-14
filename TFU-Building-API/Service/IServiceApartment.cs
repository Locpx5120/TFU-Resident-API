using Core.Model;
using TFU_Building_API.Dto;

namespace TFU_Building_API.Service
{
    public interface IServiceApartment
    {
        Task<ResponseData<PaginatedResponseDto<ApartmentServiceSummaryDto>>> GetApartmentServiceSummaryByUserId(Guid userId, int pageSize, int pageNumber);
        Task<ResponseData<List<ServiceDetailDto>>> GetServiceDetailsByApartmentId(ServiceDetailRequestDto request);
        //Task<ResponseData<PaginatedResponseDto<UnpaidServiceSummaryDto>>> GetUnpaidServiceSummaryByUserId(Guid userId, int pageSize, int pageNumber);
        Task<ResponseData<PaginatedResponseDto<UnpaidServiceSummaryDto>>> GetServiceSummaryByUserId(Guid userId, ServiceSummaryRequestDto request);

        Task<ResponseData<UnpaidServiceDetailResponseDto>> GetUnpaidServiceDetailsByApartmentId(UnpaidServiceDetailRequestDto request);



    }
}
