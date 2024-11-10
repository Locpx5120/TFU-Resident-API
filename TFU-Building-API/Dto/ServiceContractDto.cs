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
        public Guid ServiceContractId { get; set; }
        public int? Apartment { get; set; } // Room number of the apartment
        public string Building { get; set; } // Building name
        public string ServiceName { get; set; }
        public string Purpose { get; set; }
        public DateTime CreatedDate { get; set; }
        public string QuantityOrArea { get; set; }
        public decimal UnitPrice { get; set; }
        public string Status { get; set; }
        public string Note { get; set; }
        public DateTime? ProcessedDate { get; set; }
    }


    public class ServiceContractDetailRequestDto
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public string? ServiceName { get; set; } // New filter field for searching by service name
        public Guid? BuildingId { get; set; } // New filter field for filtering by building
        public Guid? ServiceTypeId { get; set; } // New filter field for filtering by service type/category
    }

    public class ServiceRequestSearchDto
    {
        public string? ServiceName { get; set; }
        public Guid? BuildingId { get; set; }
        public int? Status { get; set; }
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }


    public class ServiceRequestDto
    {
        public string ServiceType { get; set; }
        public DateTime Date { get; set; }
        public string Building { get; set; }
        public string Status { get; set; } // Status as a descriptive string
        public Guid ServiceContractId { get; set; }
    }


    //public class PaginatedResponseDto<T>
    //{
    //    public int TotalRecords { get; set; }
    //    public int PageNumber { get; set; }
    //    public int PageSize { get; set; }
    //    public List<T> Data { get; set; }
    //}


}
