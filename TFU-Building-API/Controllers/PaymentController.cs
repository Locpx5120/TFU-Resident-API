using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TFU_Building_API.Configure;
using TFU_Building_API.Dto;
using TFU_Building_API.Service;

namespace TFU_Building_API.Controllers
{
    [Route("api/payment")]
    [ApiController]
    [Authorize]
    [CustomFilter]
    public class PaymentController : ControllerBase
    {
        private readonly IPaymentHistoryService paymentHistoryService;

        public PaymentController(IPaymentHistoryService paymentHistoryService)
        {
            this.paymentHistoryService = paymentHistoryService;
        }


        [HttpPost("getQR")]
        public async Task<IActionResult> GetQR([FromBody] TransactionQRRequestDto transaction)
        {
            var result = await paymentHistoryService.GetQR(transaction);
            return StatusCode(result.Code, result);
        }

        [HttpPost("checkPayment")]
        public async Task<IActionResult> CheckPayment([FromBody] PaymentHistoryRequestDto paymentHistoryRequest)
        {
            var result = await paymentHistoryService.CheckPayment(paymentHistoryRequest);
            return StatusCode(result.Code, result);
        }

        [HttpPost("getTransactions")]
        public async Task<IActionResult> GetTransactions([FromBody] TransactionRequestDto transaction)
        {
            var result = await paymentHistoryService.GetTransactions(transaction);
            return StatusCode(result.Code, result);
        }

        [HttpPost("getTransactionDetail/{id}")]
        public async Task<IActionResult> GetTransactionDetail(Guid id)
        {
            var result = await paymentHistoryService.GetTransactionsDetail(id);
            return StatusCode(result.Code, result);
        }
    }
}
