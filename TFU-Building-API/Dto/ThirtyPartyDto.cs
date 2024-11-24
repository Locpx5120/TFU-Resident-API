namespace TFU_Building_API.Dto
{
    public class AddThirdPartyRequestDto
    {
        public string CompanyName { get; set; }
        public string ContactInfo { get; set; }
        public string StoreType { get; set; }
    }

    public class AddThirdPartyResponseDto
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
    }

    public class AddThirdPartyContactRequestDto
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public decimal Price { get; set; }
        public string NameService { get; set; }
        public Guid ThirdPartyId { get; set; }
        public Guid? BuildingId { get; set; } // Cho phép null
        public int? FloorNumber { get; set; } // Cho phép null
        public int? RoomNumber { get; set; }  // Cho phép null
    }


    public class AddThirdPartyContactResponseDto
    {
        public bool Success { get; set; }
        public string Message { get; set; }
    }

    public class ThirdPartyListRequestDto
    {
        public string? CompanyName { get; set; } // Tìm kiếm theo tên công ty
        public string? Status { get; set; } // Trạng thái: "Trong thời hạn" hoặc "Chuẩn bị hết hạn"
        public bool IsTenant { get; set; }
    }

    public class ThirdPartyListResponseDto
    {
        public string BuildingName { get; set; } // Tên tòa nhà
        public string CompanyName { get; set; } // Tên công ty
        public string ContactInfo { get; set; } // Thông tin liên hệ
        public string StoreType { get; set; } // Loại cửa hàng
        public string Status { get; set; } // Trạng thái
        public Guid ThirdPartyId { get; set; } // ID của bên thứ ba
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }

    public class ThirdPartyContractDetailDto
    {
        public string CompanyName { get; set; }      // Tên công ty
        public string BuildingName { get; set; }     // Tên tòa nhà
        public Guid ApartmentId { get; set; }
        public int? FloorNumber { get; set; }        // Số tầng
        public int? RoomNumber { get; set; }         // Số phòng
        public DateTime? StartDate { get; set; }      // Ngày thuê
        public DateTime? EndDate { get; set; }        // Ngày hết hạn
        public decimal Price { get; set; }           // Giá dịch vụ (VNĐ)
    }

    public class ThirdPartyContractInfoDto
    {
        public string BuildingName { get; set; }      // Tên building
        public Guid? ApartmentId { get; set; }
        public int? FloorNumber { get; set; }         // Số tầng
        public int? RoomNumber { get; set; }          // Số phòng
        public decimal Area { get; set; }             // Diện tích mặt bằng (m2)
        public DateTime? StartDate { get; set; }       // Ngày thuê
        public DateTime? EndDate { get; set; }         // Ngày hết hạn
        public decimal Price { get; set; }            // Giá thuê (VNĐ)
        public string Status { get; set; }            // Trạng thái hợp đồng (trong thời hạn/chuẩn bị hết hạn)
    }

    public class AddThirdPartyHireRequestDto
    {
        public string NameCompany { get; set; }
        public string ContactInfo { get; set; }
    }


    public class GetTenantRentRequestDto
    {
        public string? CompanyName { get; set; } // Search by company name
        public DateTime? EndDateFilter { get; set; } // Filter by end date
        public bool IsTenant { get; set; }
        public int PageNumber { get; set; } = 1; // Pagination - current page
        public int PageSize { get; set; } = 10; // Pagination - number of items per page
    }

    public class TenantRentResponseDto
    {
        public string CompanyName { get; set; }
        public decimal Area { get; set; }
        public string Description { get; set; }
        public DateTime EndDate { get; set; }
        public decimal RentAmount { get; set; }
    }

    public class GetThirdPartyHireRequestDto
    {
        public string CompanyName { get; set; } // Search by company name
        public string StatusFilter { get; set; } // Filter by status: "chưa thanh toán", "chuẩn bị hết hạn", "trong thời hạn"
        public int PageNumber { get; set; } = 1; // Pagination - current page
        public int PageSize { get; set; } = 10; // Pagination - number of items per page
    }

    public class ThirdPartyHireResponseDto
    {
        public Guid Id { get; set; }
        public string NameCompany { get; set; }
        public string ContactInfo { get; set; } // Assume Email or relevant field
        public string Status { get; set; } // Status description
    }

    public class ContractDetailRequestDto
    {
        public Guid ThirdPartyId { get; set; } // ID of the third party
    }

    public class ContractDetailResponseDto
    {
        public string CompanyName { get; set; } // Name of the third party
        public string NameService { get; set; }
        public int Floor { get; set; } // Floor number
        public int Room { get; set; } // Room number
        public decimal Area { get; set; } // Area in square meters
        public DateTime? StartDate { get; set; } // Contract start date
        public DateTime? EndDate { get; set; } // Contract end date
        public decimal ServicePrice { get; set; } // Price of the service
    }

    public class AddThirdPartyContractHireRequestDto
    {
        public Guid ThirdPartyId { get; set; } // ID of the third party
        public string NameService { get; set; } // Name of the service being provided
        public DateTime StartDate { get; set; } // Start date of the contract
        public DateTime EndDate { get; set; } // End date of the contract
        public decimal Price { get; set; } // Price for the service
    }

    public class AddThirdPartyContractHireResponseDto
    {
        public Guid ContractId { get; set; } // ID of the created contract
        public string Message { get; set; } // Success or failure message
    }


}
