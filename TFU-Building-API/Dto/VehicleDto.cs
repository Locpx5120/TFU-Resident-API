namespace TFU_Building_API.Dto
{

    public class VehicleServiceRequestDto
    {
        public Guid ResidentId { get; set; }
        public string VehicleType { get; set; }
        public string LicensePlate { get; set; }
        public Guid ApartmentId { get; set; }
        public Guid ServiceId { get; set; }
        public Guid PackageServiceId { get; set; }
        public string Note { get; set; }
        public DateTime StartDate { get; set; }
    }


    public class AddVehicleServiceRequestDto
    {
        public List<VehicleServiceRequestDto> Services { get; set; } = new List<VehicleServiceRequestDto>();
    }

    //public class AddVehicleServiceRequestDto
    //{
    //    public Guid ResidentId { get; set; }
    //    public Guid ApartmentId { get; set; }
    //    public string VehicleType { get; set; }
    //    public string LicensePlate { get; set; }
    //    public Guid ServiceId { get; set; }
    //    public Guid PackageServiceId { get; set; }
    //    public string Note { get; set; }
    //}

    public class AddVehicleServiceResponseDto
    {
        public bool Success { get; set; }
        public string Message { get; set; }
    }

    public class VehicleServiceDetailDto
    {
        public string BuildingName { get; set; }
        public int ApartmentNumber { get; set; }
        public string ServiceName { get; set; }
        public string Package { get; set; }
        public string VehicleType { get; set; }
        public string VehicleModel { get; set; }
        public string LicensePlate { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Note { get; set; }
    }

}
