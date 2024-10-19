using Core.Entity;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace TFU_Building_API.Model
{
    public class LoginResponseDto
    {
        public string? Token { get; set; }
        public string? RefreshToken { get; set; }
    }

    public class LoginRequestDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
        public Guid? BuildingId { get; set; }
    }

    public class ChangePasswordRequestDto
    {
        [Required]
        public string Password { get; set; }

        [Required]
        public string NewPassword { get; set; }
    }

    public class ChangePasswordResponseDto
    {

    }

    public class UserInfoResponse : MasterDataEntityBase
    {
        [JsonProperty("firstname")]
        public string FirstName { get; set; }
        [JsonProperty("lastname")]
        public string LastName { get; set; }
        public string Email { get; set; } = null!;
        public string? Phone { get; set; }
        public string? Address { get; set; }
        public Guid? RoleId { get; set; }
        public string? RoleName { get; set; }
    }

    public class RegisterRequestDto
    {
        [Required]
        public string CompanyName { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string Phone { get; set; }
    }

    public class RegisterResponseDto
    {
        public string Time { get; set; }
    }


    public class ForgotPasswordRequestDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
    public class ForgotPasswordResponseDto
    {
        public Guid UserId { get; set; }
    }

    public class ConfirmOtpRequestDto
    {
        [Required]
        public Guid UserId { get; set; }

        [Required]
        public string TypeOtp { get; set; }

        [Required]
        public string Otp { get; set; }
    }

    public class ConfirmOtpResponseDto
    {

    }
}
