namespace TFU_Building_API.Dto
{
    public class CreateNotifyRequestDto
    {
        public DateTime ApplyDate { get; set; }
        public TimeSpan ApplyTime { get; set; }
        public Guid NotifyCategoryId { get; set; }
        public Guid BuildingId { get; set; }
        public Guid RoleId { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string DetailContent { get; set; }
        public IFormFile Image { get; set; } // Ảnh được tải lên
        public int Status { get; set; }
    }

    public class CreateNotifyResponseDto
    {
        public Guid Id { get; set; }
        public string Message { get; set; }
    }

    public class NotifyResponseDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string ShortContent { get; set; }
        public DateTime? Date { get; set; }
        public TimeSpan? Time { get; set; }
        public string NotifyCategory { get; set; }
        public string BuildingName { get; set; }
        public string RoleName { get; set; }
        public string Status { get; set; }
        public string CreatedBy { get; set; }
        public string ApprovedBy { get; set; }
        public Guid? NotifyCategoryId { get; set; }
        public Guid? BuildingId { get; set; }
    }

    public class NotifyFilterRequestDto
    {
        public string? Title { get; set; }
        public Guid? NotifyCategoryId { get; set; }
        public Guid? BuildingId { get; set; }
        public string? StatusText { get; set; }
        public DateTime? ApplyDate { get; set; }
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}
