using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service;
using TFU_Resident_API.Model;

namespace Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthenController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthenController(IAuthService authService)
        {
            _authService = authService;
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
    }
}
