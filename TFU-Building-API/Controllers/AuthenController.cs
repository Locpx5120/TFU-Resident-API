using BuildingModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TFU_Building_API.Configure;
using TFU_Building_API.Core.Helper;
using TFU_Building_API.Model;
using TFU_Building_API.Service;

namespace Controllers
{
    [Route("api/auth")]
    [ApiController]
    [CustomFilter]
    public class AuthenController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly EmailService emailService;
        private readonly IConfiguration _config;

        public AuthenController(IAuthService authService, IConfiguration config)
        {
            _authService = authService;
            _config = config;
            emailService = new EmailService(_config);
        }

        [HttpPost("token")]
        public async Task<IActionResult> Login(LoginRequestDto request)
        {
            try
            {
                var response = await _authService.Login(request);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize]
        [HttpGet("token")]
        public async Task<IActionResult> GetUserInfo()
        {
            try
            {
                var response = await _authService.GetUserInfo();
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("changePassword")]
        [Authorize]
        public async Task<IActionResult> ChangePassword(ChangePasswordRequestDto request)
        {
            try
            {
                var response = await _authService.ChangePassword(request);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterRequestDto register)
        {
            try
            {
                var response = await _authService.Register(register);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("forgot")]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordRequestDto register)
        {
            try
            {
                var response = await _authService.ForgotPassword(register);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("confirm-otp")]
        public async Task<IActionResult> ConfirmOtp(ConfirmOtpRequestDto request)
        {
            try
            {
                var response = await _authService.ConfirmOtp(request);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
