namespace TFU_Building_API.Dto
{
    public class ResidentPaymentRequestDto
    {
        public string? OwnerName { get; set; } // Tên chủ căn hộ
        public bool? IsPaid { get; set; } // Trạng thái thanh toán: Đã trả hoặc Chưa trả
        public int? Month { get; set; }
        public int? Year { get; set; }
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
    public class ResidentPaymentDto
    {
        public string OwnerName { get; set; } // Tên chủ căn hộ
        public int Floor { get; set; } // Tầng
        public int RoomNumber { get; set; } // Phòng
        public Guid ApartmentId { get; set; }
        public decimal TotalAmount { get; set; } // Tiền thanh toán (VND)
        public bool IsPaid { get; set; } // Trạng thái thanh toán
    }

}
