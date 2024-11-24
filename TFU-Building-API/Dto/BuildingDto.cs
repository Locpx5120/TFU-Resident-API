using System.ComponentModel.DataAnnotations;

namespace TFU_Building_API.Dto
{
    public class BuildingRequestDto
    {
        [Required]
        public string BuildingName { get; set; }

        [Required]
        public int NumberFloor { get; set; }

        [Required]
        public int NumberApartment { get; set; }

        [Required]
        public Guid PositionId { get; set; }
    }

    public class BuildingResponseDto
    {
        public Guid Id { get; set; }
        public string BuildingName { get; set; }
    }

    public class BuildingUpdateRequestDto
    {
        [Required]
        public Guid Id { get; set; }

        [Required]
        public string BuildingName { get; set; }

        [Required]
        public int NumberFloor { get; set; }

        [Required]
        public int NumberApartment { get; set; }

        [Required]
        public Guid PositionId { get; set; }

        public bool IsActive { get; set; }
    }

    public class BuildingUpdateResponseDto
    {
        public Guid Id { get; set; }
        public string Message { get; set; }
    }


}
