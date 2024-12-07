using Core.Model;
using TFU_Building_API.Dto;

namespace TFU_Building_API.Service
{
    public interface IPaymentHistoryService
    {
        public Task<ResponseData<PaymentHistoryResponseDto>> CheckPayment(string type, Guid id);
    }
}
