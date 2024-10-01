using Core.Entity;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace TFU_Resident_API.Model
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
}
