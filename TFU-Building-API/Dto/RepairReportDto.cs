namespace TFU_Building_API.Dto
{
    public class AddRepairReportServiceRequestDto
    {
        public List<RepairReportServiceRequestDto> Services { get; set; } = new List<RepairReportServiceRequestDto>();
    }

    public class RepairReportServiceRequestDto
    {
        public Guid ApartmentId { get; set; }
        public string Note { get; set; }
        public Guid ResidentId { get; set; }
        public Guid ServiceId { get; set; }
    }
    public class AddRepairReportServiceResponseDto
    {
        public bool Success { get; set; }
        public string Message { get; set; }
    }

    public class UpdateRepairReportServiceRequestDto
    {
        public Guid ServiceContractId { get; set; }
        public int Status { get; set; } // Use constants from ServiceContractStatus
        public string Note { get; set; }
    }
}
