using System.ComponentModel.DataAnnotations;

namespace TFU_Building_API.Dto
{
    public class ApartmentServiceSummaryDto
    {
        public Guid ApartmentId { get; set; }
        public int RoomNumber { get; set; }
        public int TotalServices { get; set; }
    }

    public class ApartmentServiceRequestDto
    {
        [Required]
        public Guid UserId { get; set; }

        [Range(1, int.MaxValue)]
        public int PageSize { get; set; } = 10;

        [Range(1, int.MaxValue)]
        public int PageNumber { get; set; } = 1;
    }

    public class ServiceDetailDto
    {
        public string ServiceName { get; set; }
        public string Description { get; set; }
        public string QuantityOrArea { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal TotalPrice { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }

    public class ServiceDetailRequestDto
    {
        [Required]
        public Guid ApartmentId { get; set; }

        public string ServiceType { get; set; } // Loại dịch vụ (ví dụ: "Gửi xe", "Dịch vụ phòng")
    }

    //public class PaginatedResponseDto<T>
    //{
    //    public int TotalRecords { get; set; }
    //    public List<T> Data { get; set; }
    //}
}
