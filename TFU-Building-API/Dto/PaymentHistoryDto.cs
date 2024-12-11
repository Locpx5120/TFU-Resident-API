namespace TFU_Building_API.Dto
{
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

    public class PaymentHistoryRequestDto
    {
        public string Type { get; set; } // Thể loại giao dịch
        public Guid TransactionMapId { get; set; } // id của giao dịch 
        public double Price { get; set; } // số tiền
        public string Content { get; set; } // Nội dung giao dịch
        public string Bank { get; set; } // ngân hàng
        public string AccountNumber { get; set; } // tài khoản nhận tiền
    }

    public class PaymentHistoryResponseDto
    {
        public bool Result { get; set; }
    }

    public class TransactionHistoryResponseDto
    {
        public Guid Id { get; set; }
        public string Type { get; set; } // Thể loại giao dịch
        public Guid? TransactionMapId { get; set; } // id của giao dịch 
        public decimal? Price { get; set; } // số tiền
        public string Content { get; set; } // Nội dung giao dịch
        public string Bank { get; set; } // ngân hàng
        public string AccountNumber { get; set; } // tài khoản nhận tiền
    }
    public class TransactionResponseDto
    {
        public List<TransactionHistoryResponseDto>? TransactionHistories { get; set; } = new List<TransactionHistoryResponseDto>();
        public double? Total { get; set; } = 0; // Tổng tiền
        public double? Pay { get; set; } = 0; // Thu
        public double? Transfer { get; set; } = 0; // Chi
    }
    public class TransactionRequestDto
    {
        public DateTime? To { get; set; } = null;
        public DateTime? From { get; set; } = null;
    }
}
