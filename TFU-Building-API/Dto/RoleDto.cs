using System.ComponentModel.DataAnnotations;

namespace TFU_Building_API.Dto
{
    public class RoleRequestDto
    {
        [Required]
        public string Name { get; set; }  // Tên của vai trò
    }

    public class RoleResponseDto
    {
        public Guid Id { get; set; }      // Id của vai trò sau khi tạo
        public string Name { get; set; }  // Tên của vai trò
    }

}
