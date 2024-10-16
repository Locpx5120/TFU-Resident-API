using System.ComponentModel.DataAnnotations;

namespace TFU_Resident_API.Dto
{
    public class ViewManagerUserRequest
    {
        public string? Name { get; set; }
        public Guid? RoleId { get; set; }
    }

    public class UserDto
    {
        public Guid Id { get; set; }
        public string UserName { get; set; } = "";
        public string RoleName { get; set; } = "";
        public Guid RoleId { get; set; }
        public string Phone { get; set; } = "";
        public string Email { get; set; } = "";
        public DateTime Dob { get; set; }
        public string NumberCccd { get; set; } = "";
        public string Genders { get; set; } = "Male";
    }

    public class UserDeleteRequest
    {
        [Required(ErrorMessage = "Id is required.")]
        public Guid Id { get; set; }
    }

    public class UserUpdateRequest
    {
        [Required(ErrorMessage = "Id is required.")]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Name is required.")]
        [MaxLength(100, ErrorMessage = "Name cannot exceed 100 characters.")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "RoleId is required.")]
        public Guid RoleId { get; set; }
        public string Phone { get; set; } = "";

        [EmailAddress(ErrorMessage = "Invalid email address format.")]
        public string Email { get; set; }

        [DataType(DataType.Date, ErrorMessage = "Invalid date format.")]
        public DateTime Dob { get; set; }

        [MaxLength(12, ErrorMessage = "NumberCccd cannot exceed 12 characters.")]
        public string NumberCccd { get; set; }

        [Required(ErrorMessage = "Gender is required.")]
        [RegularExpression("Male|Female", ErrorMessage = "Gender must be Male, Female, or Other.")]
        public string Genders { get; set; }
    }


    public class UserCreateRequest
    {
        [Required(ErrorMessage = "Name is required.")]
        [MaxLength(100, ErrorMessage = "Name cannot exceed 100 characters.")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "RoleId is required.")]
        public Guid RoleId { get; set; }
        public string Phone { get; set; } = "";

        [EmailAddress(ErrorMessage = "Invalid email address format.")]
        public string Email { get; set; }

        [DataType(DataType.Date, ErrorMessage = "Invalid date format.")]
        public DateTime Dob { get; set; }

        [MaxLength(12, ErrorMessage = "NumberCccd cannot exceed 12 characters.")]
        public string NumberCccd { get; set; }

        [Required(ErrorMessage = "Gender is required.")]
        [RegularExpression("Male|Female", ErrorMessage = "Gender must be Male, Female, or Other.")]
        public string Genders { get; set; }
    }
}
