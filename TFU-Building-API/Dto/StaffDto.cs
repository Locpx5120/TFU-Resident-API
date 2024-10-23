using System.ComponentModel.DataAnnotations;

namespace TFU_Building_API.Dto
{
    public class StaffRequestDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public Guid RoleId { get; set; }
    }

    public class StaffResponseDto {
        public Guid Id { get; set; }
    }

    public class StaffDeleteRequestDto
    {
        public Guid StaffId { get; set; }
    }

    public class StaffUpdateRequestDto
    {
        public Guid StaffId { get; set; }
        public Guid RoleId { get; set; }
    }

    public class StaffSearchRequestDto
    {
        public string? Name { get; set; }
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }

    public class StaffListResponseDto
    {
        public string FullName { get; set; }
        public string Department { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public DateTime HireDate { get; set; }
        public Guid Id { get; set; }
        public int TotalRecords { get; set; }
    }

    public class StaffInfoResponseDto
    {
        public Guid Id { get; set; }
        public string Email { get; set; }
        public string Department { get; set; }
    }
}
