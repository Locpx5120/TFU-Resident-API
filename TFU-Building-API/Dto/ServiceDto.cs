﻿using System.ComponentModel.DataAnnotations;

namespace TFU_Building_API.Dto
{
    public class ApartmentServiceSummaryDto
    {
        public Guid ApartmentId { get; set; }
        public string BuildingName { get; set; }  // Added for building name
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
        public decimal? UnitPrice { get; set; }
        public decimal? TotalPrice { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }

    public class ServiceDetailRequestDto
    {
        [Required]
        public Guid ApartmentId { get; set; }

        public string ServiceType { get; set; } // Loại dịch vụ (ví dụ: "Gửi xe", "Dịch vụ phòng")

        public DateTime? StartDateFrom { get; set; } // Ngày bắt đầu từ
        public DateTime? StartDateTo { get; set; }   // Ngày bắt đầu đến
    }

    public class UnpaidServiceSummaryDto
    {
        public Guid ApartmentId { get; set; }
        public Guid BuildingId { get; set; } // Added for building ID
        public string BuildingName { get; set; }
        public int RoomNumber { get; set; }
        public int TotalServices { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }
        public string PaymentStatus { get; set; }
    }


    public class ServiceSummaryRequestDto
    {
        public int PageSize { get; set; } = 10;
        public int PageNumber { get; set; } = 1;
        public Guid? BuildingIdFilter { get; set; } // Filter by Building ID
        public Guid? ApartmentIdFilter { get; set; } // Filter by Apartment ID
        public string? PaymentStatusFilter { get; set; }
        public int? MonthFilter { get; set; }
        public int? YearFilter { get; set; }
    }




    public class UnpaidServiceDetailDto
    {
        public Guid InvoiceId { get; set; }
        public string ServiceName { get; set; }     // Tên dịch vụ
        public string Description { get; set; }     // Mô tả
        public string QuantityOrArea { get; set; }  // Số lượng hoặc diện tích
        public decimal UnitPrice { get; set; }      // Giá tiền
        public decimal TotalPrice { get; set; }     // Tổng tiền
        public string PaymentStatus { get; set; }
        public DateTime? PaymentDate { get; set; }
    }

    public class UnpaidServiceDetailResponseDto
    {
        public List<UnpaidServiceDetailDto> Services { get; set; }  // Danh sách chi tiết từng dịch vụ
        public decimal TotalAmount { get; set; }                    // Tổng giá trị của tất cả dịch vụ
    }

    public class UnpaidServiceDetailRequestDto
    {
        [Required]
        public Guid ApartmentId { get; set; }

        public string ServiceType { get; set; } // Loại dịch vụ (ví dụ: "Gửi xe", "Dịch vụ phòng")
        public int Month { get; set; }
        public int Year { get; set; }
    }

    public class ServiceDto
    {
        public Guid Id { get; set; }
        public string ServiceName { get; set; }
    }

    public class ServiceResponseDto
    {
        public Guid Id { get; set; }
        public string ServiceName { get; set; }
        public string Description { get; set; }
        public decimal UnitPrice { get; set; }
        public string Unit { get; set; }
        public bool IsPackageAllowed { get; set; }
    }



    //public class PaginatedResponseDto<T>
    //{
    //    public int TotalRecords { get; set; }
    //    public List<T> Data { get; set; }
    //}
}
