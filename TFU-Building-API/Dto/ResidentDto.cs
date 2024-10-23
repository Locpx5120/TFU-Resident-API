using System.ComponentModel.DataAnnotations;

namespace TFU_Building_API.Dto
{
    public class ResidentRequestDto
    {
        [Required]
        public string Name { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [RegularExpression(@"^\d{10}$", ErrorMessage = "Phone number must be 10 digits.")]
        public string Phone { get; set; }

        [Required]
        public Guid OwnerShipId { get; set; }
    }

    public class ResidentResponseDto
    {
        public Guid Id { get; set; }
    }

    public class ResidentUpdateRequestDto
    {
        [Required]
        public Guid Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [RegularExpression(@"^\d{10}$", ErrorMessage = "Phone number must be 10 digits.")]
        public string Phone { get; set; }

    }

    public class ResidentSearchRequestDto
    {
        public Guid OwnershipId { get; set; }
        public string? Name { get; set; }
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }

    public class ResidentListResponseDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public DateTime? RegistratorDate { get; set; }
    }

    public class PagedResidentListResponseDto
    {
        public List<ResidentListResponseDto> Residents { get; set; }
        public int TotalRecords { get; set; }
    }

    public class ResidentDeleteRequestDto
    {
        public Guid ResidentId { get; set; }
    }

    public class ResidentInfoResponseDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
    }
}
