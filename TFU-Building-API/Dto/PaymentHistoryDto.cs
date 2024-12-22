namespace TFU_Building_API.Dto
{
    /// <summary>
    /// type = TRSCINA Thanh toán dịch vụ phòng theo căn hộ danh sách id voince
    /// </summary>
    public class TransactionQRRequestDto
    {
        public string Type { get; set; } // Thể loại giao dịch
        public Guid? TransactionMapId { get; set; } // id của giao dịch 
        public List<Guid>? InvoiceId { get; set; } // id của danh sách giao dịch 
    }
    public class TransactionQRResponseDto
    {
        public Guid Id { get; set; }
        public string ImgQR { get; set; } // Nội dung giao dịch
    }
    public class RequestBodyItem
    {
        public Guid Id { get; set; }
    }

    public class PaymentHistoryRequestDto
    {
        public string Type { get; set; } // Thể loại giao dịch
        public Guid TransactionMapId { get; set; } // id của giao dịch 
        //public double Price { get; set; } // số tiền
        //public string Content { get; set; } // Nội dung giao dịch
        //public string Bank { get; set; } // ngân hàng
        //public string AccountNumber { get; set; } // tài khoản nhận tiền
    }

    public class PaymentHistoryResponseDto
    {
        public bool Result { get; set; }
    }

    public class TransactionHistoryResponseDto
    {
        public string Service { get; set; } // dich vu
        public string SentUser { get; set; } // nguoi gui
        public string ReciveUser { get; set; } // nguoi nhan
        public string Status { get; set; } // dich vu
        public decimal Amount { get; set; } // dich vu
        public DateTime CreateAt { get; set; } // 
        public int ApartmentFloorNumber { get; set; } // phong
        public int ApartmentRoomNumber { get; set; } // phong
        public string BuildingName { get; set; } // toa nha
        public Guid BuildingId { get; set; } // toa nha
        public Guid ApartmentId { get; set; } // toa nha

        public Guid ServiceId { get; set; }
        //public Guid Id { get; set; }
        public string Type { get; set; } // Thể loại giao dịch
        public Guid? TransactionMapId { get; set; } // id của giao dịch 
        //public decimal? Price { get; set; } // số tiền
        public string Content { get; set; } // Nội dung giao dịch
        public string Bank { get; set; } // ngân hàng
        public string AccountNumber { get; set; } // tài khoản nhận tiền
    }
    public class TransactionResponseDto
    {
        public List<TransactionHistoryResponseDto>? TransactionHistories { get; set; } = new List<TransactionHistoryResponseDto>();
        public List<TransactionTransferResponseDto>? TransactionTransferResponseDtos { get; set; } = new List<TransactionTransferResponseDto>();
        public decimal? Total { get; set; } = 0; // Tổng tiền
        public decimal? Pay { get; set; } = 0; // Thu
        public decimal? Transfer { get; set; } = 0; // Chi
    }

    public class TransactionTransferResponseDto
    {
        public Guid Id { get; set; }
        public string NameService { get; set; }
        public string Type { get; set; } // Thể loại giao dịch
        public decimal? Price { get; set; } // số tiền
        public int ApartmentFloorNumber { get; set; } // phong
        public int ApartmentRoomNumber { get; set; } // phong
        public string BuildingName { get; set; } // toa nha
        public Guid BuildingId { get; set; } // toa nha
        public Guid ApartmentId { get; set; } // toa nha
    }

    public class TransactionRequestDto
    {
        public DateTime? To { get; set; } = null;
        public DateTime? From { get; set; } = null;
        public Guid? BuildingId { get; set; } = null;// toa nha
        public Guid? ApartmentId { get; set; } = null;// toa nha
    }

}
