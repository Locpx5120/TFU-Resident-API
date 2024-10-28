namespace TFU_Building_API.Dto
{
    public class CreateServiceContractRequestDto
    {
        public Guid ApartmentId { get; set; }
        public Guid ServiceId { get; set; }
        public Guid PackageServiceId { get; set; }
        public int Quantity { get; set; }
        public DateTime StartDate { get; set; }
        public string Note { get; set; }
    }

    public class ServiceContractDetailDto
    {
        public int? Apartment { get; set; } // Tên căn hộ
        public string ServiceName { get; set; }
        public string Purpose { get; set; }  // Mục đích
        public DateTime CreatedDate { get; set; }
        public string QuantityOrArea { get; set; }  // Số lượng/m2
        public decimal UnitPrice { get; set; }  // Giá tiền/tháng
        public string Status { get; set; }
        public string Note { get; set; }
        public DateTime? ProcessedDate { get; set; }  // Ngày xử lý
    }

    public class ServiceContractDetailRequestDto
    {
        public int PageNumber { get; set; } = 1; // Trang mặc định là 1
        public int PageSize { get; set; } = 10;  // Kích thước mặc định là 10
    }

    //public class PaginatedResponseDto<T>
    //{
    //    public int TotalRecords { get; set; }
    //    public int PageNumber { get; set; }
    //    public int PageSize { get; set; }
    //    public List<T> Data { get; set; }
    //}


}
