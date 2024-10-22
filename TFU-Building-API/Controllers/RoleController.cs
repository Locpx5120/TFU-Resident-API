using Microsoft.AspNetCore.Mvc;
using TFU_Building_API.Configure;
using TFU_Building_API.Dto;
using TFU_Building_API.Service;

[Route("api/role")]
[ApiController]
[CustomFilter]
public class RoleController : ControllerBase
{
    private readonly IRoleService _roleService;

    public RoleController(IRoleService roleService)
    {
        _roleService = roleService;
    }

    [HttpPost("add")]
    public async Task<IActionResult> AddRole([FromBody] RoleRequestDto request)
    {
        var response = await _roleService.AddRole(request);
        if (response.Success)
        {
            return Ok(response);
        }
        return BadRequest(response);
    }

    [HttpGet("getRoles")]
    public async Task<IActionResult> GetRoles()
    {
        var response = await _roleService.GetRoles();
        if (response.Success)
        {
            return Ok(response);
        }
        return BadRequest(response);
    }
}
