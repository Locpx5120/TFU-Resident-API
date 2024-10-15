using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TFU_Resident_API.Dto;
using TFU_Resident_API.Services;

namespace TFU_Resident_API.Controllers
{
    [Route("api/investor")]
    [ApiController]
    [Authorize]
    public class InvestorController : ControllerBase
    {
        private readonly IInvestorService _investorService;

        public InvestorController(IInvestorService investorService)
        {
            this._investorService = investorService;
        }

        [HttpPost("createInvestor")]
        public async Task<IActionResult> CreateInvestor(CreateInvestorDto createInvestorDto)
        {
            var result = await _investorService.CreateInvestor(createInvestorDto);
            return Ok(result);
        }

        // Quản lý nhanh
        [HttpPost("viewManager")]
        public async Task<IActionResult> ViewManager()
        {
            var result = await _investorService.ViewManager();
            return Ok(result);
        }
    }
}
