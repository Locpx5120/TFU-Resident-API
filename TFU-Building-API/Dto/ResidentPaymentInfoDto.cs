namespace TFU_Building_API.Dto
{
    public class ResidentPaymentInfoDto
    {
        public Guid BuildingId
        {
            get; set;
        }
        public string ResidentName { get; set; }
        public string BuildingName { get; set; }
        public int RoomNumber { get; set; }
        public decimal TotalAmount { get; set; }
        public string PaymentStatus { get; set; }
        public bool PaidStatus { get; set; }
        public DateTime? PaymentDate { get; set; }
        public Guid InvoiceId { get; set; }
    }

    public class PaymentFilterDto
    {
        public string? ResidentName { get; set; }
        public Guid? BuildingId { get; set; }
        public bool? IsPaid { get; set; }
        public DateTime? PaymentMonth { get; set; } // This could be Year/Month filtering
    }
}
