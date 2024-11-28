using BuildingModels;
using fake_tool.Helpers;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using TFU_Building_API.Core.Struct;

namespace TFU_Building_API.Helpers
{
    public class Jwt
    {

        private readonly IConfiguration _config;

        public Jwt(AppSettings appSettings, IConfiguration config)
        {
            this._config = config;
        }

        //public string GenerateToken(User account, string role, Guid buildingPermalink)
        //{
        //    var jwtTokenHandler = new JwtSecurityTokenHandler();
        //    var secretKeyBytes = Encoding.ASCII.GetBytes(_config[AppSetting.AppSettings.SecretKey]);
        //    var tokenDescription = new SecurityTokenDescriptor
        //    {
        //        Subject = new ClaimsIdentity(new[]
        //        {
        //                new Claim(ClaimTypes.NameIdentifier, account.Id.ToString()),
        //                new Claim(ClaimTypes.Email, account.Email??""),
        //                new Claim(ClaimTypes.Role, role),
        //                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        //                new Claim("BuildingPermalink", buildingPermalink.ToString())
        //        }),
        //        Expires = DateTime.UtcNow.AddDays(5), // Thiết lập thời gian hết hạn sau 5 ngay
        //        SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(secretKeyBytes), SecurityAlgorithms.HmacSha256Signature)
        //    };

        //    var token = jwtTokenHandler.CreateToken(tokenDescription);
        //    var accessToken = jwtTokenHandler.WriteToken(token);
        //    return accessToken;
        //}

        public string GenerateToken(object account, string role, Guid buildingPermalink)
        {
            var jwtTokenHandler = new JwtSecurityTokenHandler();
            var secretKeyBytes = Encoding.ASCII.GetBytes(_config[AppSetting.AppSettings.SecretKey]);

            // Khởi tạo các claim chung
            var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Role, role),
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                        new Claim("BuildingPermalink", buildingPermalink.ToString())
                    };

            // Thêm các claim dựa trên kiểu của account
            if (account is Resident resident)
            {
                claims.Add(new Claim(ClaimTypes.NameIdentifier, resident.Id.ToString()));
                claims.Add(new Claim(ClaimTypes.Email, resident.Email ?? ""));
            }
            else if (account is Staff staff)
            {
                claims.Add(new Claim(ClaimTypes.NameIdentifier, staff.Id.ToString()));
                claims.Add(new Claim(ClaimTypes.Email, staff.Email ?? ""));
            }
            else
            {
                throw new ArgumentException("Account must be of type Resident or Staff", nameof(account));
            }

            // Thiết lập cấu hình token
            var tokenDescription = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddDays(5), // Thiết lập thời gian hết hạn sau 5 ngày
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(secretKeyBytes), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = jwtTokenHandler.CreateToken(tokenDescription);
            var accessToken = jwtTokenHandler.WriteToken(token);
            return accessToken;
        }




        public string GenerateRefreshToken()
        {
            var random = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(random);
                return Convert.ToBase64String(random);
            }
        }

    }
}
