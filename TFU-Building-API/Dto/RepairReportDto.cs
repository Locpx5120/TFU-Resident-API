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
        public string? Note { get; set; } = string.Empty;
        public string? NoteDetail { get; set; } = string.Empty;
        public string? NoteFeedbackCuDan { get; set; } = string.Empty;
        public string? NoteKyThuat { get; set; } = string.Empty;
        public string? NoteFeedbackHanhChinh { get; set; } = string.Empty;
        public DateTime? StartDate { get; set; } = DateTime.Now;
        public double? ServicePrice { get; set; } = 0;
        public Guid? StaffId { get; set; }
    }

    public class RepairReportServiceDetailDto
    {
        public Guid ContractId { get; set; }
        public string BuildingName { get; set; }
        public int ApartmentNumber { get; set; }
        public string ServiceName { get; set; }
        public string Package { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndDate { get; set; }
        public DateTime? StaffEndDate { get; set; }
        public string Note { get; set; }
        public string? NoteDetail { get; set; } = string.Empty;
        public string? NoteFeedbackCuDan { get; set; } = string.Empty;
        public string? NoteKyThuat { get; set; } = string.Empty;
        public string? NoteFeedbackHanhChinh { get; set; } = string.Empty;
        public double? ServicePrice { get; set; } = 0;
        public int? Status { get; set; }
    }
}
