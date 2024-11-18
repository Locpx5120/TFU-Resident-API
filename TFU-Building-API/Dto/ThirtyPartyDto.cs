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
        public int? FloorNumber { get; set; }         // Số tầng
        public int? RoomNumber { get; set; }          // Số phòng
        public decimal Area { get; set; }             // Diện tích mặt bằng (m2)
        public DateTime? StartDate { get; set; }       // Ngày thuê
        public DateTime? EndDate { get; set; }         // Ngày hết hạn
        public decimal Price { get; set; }            // Giá thuê (VNĐ)
        public string Status { get; set; }            // Trạng thái hợp đồng (trong thời hạn/chuẩn bị hết hạn)
    }
}
