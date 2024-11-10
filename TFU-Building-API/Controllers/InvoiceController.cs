using Core.Enums;
using Microsoft.AspNetCore.Mvc;
using TFU_Building_API.Configure;
using TFU_Building_API.Dto;
using TFU_Building_API.Service;

namespace TFU_Building_API.Controllers
{
    [ApiController]
    [Route("api/invoice")]
    [CustomFilter]
    public class InvoiceController : ControllerBase
    {
        private readonly IInvoiceService _invoiceService;

        public InvoiceController(IInvoiceService invoiceService)
        {
            _invoiceService = invoiceService;
        }

        [HttpPost("add")]
        public async Task<IActionResult> AddInvoicesForApartment([FromBody] CreateInvoiceRequestDto request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var response = await _invoiceService.AddInvoicesForApartment(request);
            if (!response.Success)
            {
                return BadRequest(response);
            }

            return Ok(response);
        }

        /// <summary>
        /// Xử lý thanh toán cho hóa đơn và tạo mã QR thanh toán.
        /// </summary>
        /// <param name="request">Yêu cầu thanh toán hóa đơn.</param>
        /// <returns>Kết quả thanh toán và mã QR.</returns>
        [HttpPost("process-payment")]
        public async Task<IActionResult> ProcessInvoicePayment([FromBody] InvoicePaymentRequestDto request)
        {
            var result = await _invoiceService.ProcessInvoicePaymentAsync(request);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        /// <summary>
        /// Lấy danh sách tiền thu từ cư dân
        /// </summary>
        /// <param name="filter">Bộ lọc thanh toán</param>
        /// <returns>Danh sách thông tin thanh toán</returns>
        [HttpGet("resident-payments")]
        public async Task<IActionResult> GetResidentPayments([FromQuery] PaymentFilterDto filter)
        {
            var result = await _invoiceService.GetResidentPaymentListAsync(filter);

            if (result.Success)
            {
                return Ok(result);
            }
            else
            {
                return StatusCode(result.Code == (int)ErrorCodeAPI.SystemIsError ? 500 : 404, result);
            }
        }
    }
}
