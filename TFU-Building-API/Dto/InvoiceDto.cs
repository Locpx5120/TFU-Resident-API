using System.ComponentModel.DataAnnotations;

namespace TFU_Building_API.Dto
{
    public class CreateInvoiceRequestDto
    {
        [Required]
        public Guid ApartmentId { get; set; }
    }

    public class InvoicePaymentRequestDto
    {
        public Guid InvoiceId { get; set; }
        public string BankAccountName { get; set; }
        public string BankAccountNumber { get; set; }
        public string BankName { get; set; }
        public decimal Amount { get; set; }
        public string TransactionContent { get; set; }
    }

    public class InvoicePaymentResponseDto
    {
        public bool Success { get; set; }
        public string QRCodeUrl { get; set; }
        public string Message { get; set; }
    }
}
