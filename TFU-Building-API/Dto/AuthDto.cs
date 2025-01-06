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

    public class UserInfoResponse
    {
        public string FullName { get; set; }
        public string Email { get; set; }
        public string? PhoneNumber { get; set; }
        public DateTime? Birthday { get; set; }
    }

    public class UserInfoRequestDto
    {
        public string FullName { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime Birthday { get; set; }
    }

    public class UserChangePassRequestDto
    {
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }
        public string ConfirmPassword { get; set; }
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
