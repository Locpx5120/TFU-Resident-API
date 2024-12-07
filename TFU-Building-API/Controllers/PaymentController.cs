using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace TFU_Building_API.Controllers
{
    [Route("api/payment")]
    [ApiController]
    [Authorize]
    public class PaymentController : ControllerBase
    {

        //[HttpGet("details/{id}")]
        //public async Task<IActionResult> GetPaymentDetail()
        //{
        //    var result = await _apartmentService.GetApartmentDetailsByApartmentIdAsync(apartmentId, memberName);
        //    return StatusCode(result.Code, result);
        //}
    }
}
