
using Core.Model;
using TFU_Building_API.Dto;

namespace TFU_Building_API.Service
{
    public interface IInvoiceService
    {
        Task<ResponseData<string>> AddInvoicesForApartment(CreateInvoiceRequestDto request);
        Task<ResponseData<InvoicePaymentResponseDto>> ProcessInvoicePaymentAsync(InvoicePaymentRequestDto request);


    }
}
