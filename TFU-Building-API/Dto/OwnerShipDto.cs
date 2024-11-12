using System.ComponentModel.DataAnnotations;

namespace TFU_Building_API.Dto
{
    public class OwnerShipRequestDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public int RoomNumber { get; set; }

        [Required]
        public int FloorNumber { get; set; }

        public Guid BuildingId { get; set; }
    }

    public class OwnerShipResponseDto
    {
        public Guid Id { get; set; }
    }

    public class OwnerShipUpdateRequestDto
    {
        [Required]
        public Guid Id { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public int RoomNumber { get; set; }

        [Required]
        public int FloorNumber { get; set; }

        public Guid BuildingId { get; set; }
    }

    public class OwnerShipDeleteRequestDto
    {
        [Required]
        public Guid OwnerShipId { get; set; }
    }

    public class OwnerShipListResponseDto
    {
        public string FullName { get; set; }
        public int FloorNumber { get; set; }
        public int RoomNumber { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public Guid Id { get; set; }
    }

    public class OwnerShipSearchRequestDto
    {
        public string? Name { get; set; }
        public int PageSize { get; set; } = 10;
        public int PageNumber { get; set; } = 1;
    }

    public class PaginatedResponseDto<T>
    {
        public int TotalRecords { get; set; }
        public List<T> Data { get; set; }
    }

    public class OwnershipInfoResponseDto
    {
        public Guid Id { get; set; }
        public string Email { get; set; }
        public int FloorNumber { get; set; }
        public int RoomNumber { get; set; }
    }
}
