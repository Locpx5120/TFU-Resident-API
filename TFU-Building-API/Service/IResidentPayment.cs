using Core.Model;
using TFU_Building_API.Dto;

namespace TFU_Building_API.Service
{
    public interface IResidentPayment
    {
        Task<ResponseData<PaginatedResponseDto<ResidentPaymentDto>>> GetResidentPayments(ResidentPaymentRequestDto request);
    }
}
