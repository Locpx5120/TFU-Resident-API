using Core.Model;
using TFU_Building_API.Dto;

namespace TFU_Building_API.Service
{
    public interface IPaymentHistoryService
    {
        public Task<ResponseData<TransactionQRResponseDto>> GetQR(TransactionQRRequestDto transaction);
        public Task<ResponseData<PaymentHistoryResponseDto>> CheckPayment(PaymentHistoryRequestDto paymentHistoryRequest);
        public Task<ResponseData<TransactionResponseDto>> GetTransactions(TransactionRequestDto transaction);

    }
}
