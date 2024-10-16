using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TFU_Resident_API.Dto;
using TFU_Resident_API.Services;

namespace TFU_Resident_API.Controllers
{
    [Route("api/user")]
    [ApiController]
    [Authorize]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            this._userService = userService;
        }

        [HttpPost("viewManager")]
        public async Task<IActionResult> ViewManager(ViewManagerUserRequest request)
        {
            var result = await _userService.ViewManager(request);
            return Ok(result);
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create(UserCreateRequest request)
        {
            var result = await _userService.Create(request);
            return Ok(result);
        }

        [HttpPost("update")]
        public async Task<IActionResult> Update(UserUpdateRequest request)
        {
            var result = await _userService.Update(request);
            return Ok(result);
        }

        [HttpPost("delete")]
        public async Task<IActionResult> Delete(UserDeleteRequest request)
        {
            var result = await _userService.Delete(request);
            return Ok(result);
        }
    }
}
