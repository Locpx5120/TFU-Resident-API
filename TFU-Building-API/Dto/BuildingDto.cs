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
        public string Address { get; set; }
        [Required]
        public DateTime CreateAt { get; set; } // thời điểm toà nhà đc tạo
    }

    public class BuildingResponseDto
    {
        public Guid Id { get; set; }
        public string BuildingName { get; set; }
        public int NumberFloor { get; set; }
        public int NumberApartment { get; set; }
        public string Address { get; set; }
        public DateTime? CreateAt { get; set; }
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
        public bool IsActive { get; set; }
        [Required]
        public string Address { get; set; }
        [Required]
        public DateTime CreateAt { get; set; } // thời điểm toà nhà đc tạo
    }

    public class BuildingUpdateResponseDto
    {
        public Guid Id { get; set; }
        public string Message { get; set; }
    }


}
