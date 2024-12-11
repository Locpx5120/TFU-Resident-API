using BuildingModels;
using Constant;
using Core.Enums;
using Core.Model;
using fake_tool.Helpers;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using TFU_Building_API.Core.Handler;
using TFU_Building_API.Core.Infrastructure;
using TFU_Building_API.Dto;

namespace TFU_Building_API.Service.impl
{
    public class PaymentHistoryService : BaseHandler, IPaymentHistoryService
    {
        private static readonly HttpClient client = new HttpClient();
        private string BankId = "970415";
        private string SoTaiKhoan = "973659865";
        private string TotalAmount = "100";
        private string NoiDung = "Thanh toan don han";
        private string AccountName = "Hoang Tuan Kiet";
        private string URL = "https://oauth.casso.vn/v2/transactions"; // Thay bằng URL thật
        private string APIKEY = "AK_CS.f5b8bUewYuYzaA8TGwIyzNBVD1uZvrEz8PqdFas6hPzOC7rHERe7Pm7lygLJJckog0R4IWH"; // token

        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserIdentity _userIdentity;

        public PaymentHistoryService(IUnitOfWork unitOfWork, IHttpContextAccessor httpContextAccessor,
           IUserIdentity userIdentity)
           : base(unitOfWork, httpContextAccessor)
        {
            _unitOfWork = unitOfWork;
            _userIdentity = userIdentity;
        }

        public async Task<ResponseData<PaymentHistoryResponseDto>> CheckPayment(PaymentHistoryRequestDto paymentHistoryRequest)
        {

            string content = ConvertContent(paymentHistoryRequest.Type, paymentHistoryRequest.TransactionMapId);
            bool check = await CheckBank(content, paymentHistoryRequest.Price);
            if (!check)
            {
                return new ResponseData<PaymentHistoryResponseDto>
                {
                    Success = false,
                    Message = "Khong tim thay thanh toan nay",
                    Data = new PaymentHistoryResponseDto { Result = false },
                    Code = (int)ErrorCodeAPI.OK
                };
            }
            switch (paymentHistoryRequest.Type)
            {
                case Constants.TRANS_SERVICE_INVOICE_ALL:

                    break;
            }

            return new ResponseData<PaymentHistoryResponseDto>
            {
                Success = true,
                Message = "Apartment information retrieved successfully.",
                Data = new PaymentHistoryResponseDto { Result = false },
                Code = (int)ErrorCodeAPI.OK
            };
        }

        public async Task<bool> CheckBank(string noiDung, double price)
        {
            try
            {
                // Tạo request với header "Authorization: apikey <apiKey>"
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Add("Authorization", "apikey " + APIKEY);

                HttpResponseMessage response = await client.GetAsync(URL);
                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception("Unexpected code " + response.StatusCode);
                }

                // Đọc dữ liệu từ response
                string responseData = await response.Content.ReadAsStringAsync();

                // Parse JSON response nếu cần
                JObject json = JObject.Parse(responseData);
                if (json["data"]?["records"] != null)
                {
                    var recordsArray = json["data"]["records"];
                    foreach (var recordJson in recordsArray)
                    {
                        int id = recordJson.Value<int>("id");
                        string tid = recordJson.Value<string>("tid");
                        int amount = recordJson.Value<int>("amount");
                        string description = recordJson.Value<string>("description");

                        // Kiểm tra mô tả và giá trị
                        if (!string.IsNullOrEmpty(description) && noiDung.Length > 25)
                        {
                            if (description.ToLower().Contains(noiDung.ToLower()) && price == amount)
                            {
                                return true;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Log hoặc xử lý exception nếu cần
                //Console.WriteLine(ex.Message);
            }

            return false;
        }

        public async Task<ResponseData<TransactionResponseDto>> GetTransactions(TransactionRequestDto transactionRequest)
        {
            try
            {
                TransactionResponseDto transactionResponseDto = new TransactionResponseDto();
                // Stage 1: Fetch data from the database
                var query = await _unitOfWork.TransactionRepository.GetQuery(x => x.IsDeleted == false
                && !x.Status.Equals(Constants.TRANS_STATUS_LOG_INIT)
                && !x.Status.Equals(Constants.TRANS_STATUS_LOG_DONE)
                ).ToListAsync();


                // Stage 3: In-memory transformations
                var resultData = new List<TransactionHistoryResponseDto>();
                foreach (var item in query.ToList())
                {
                    var transaction = new TransactionHistoryResponseDto
                    {
                        Id = item.Id,
                        Type = item.Type,
                        TransactionMapId = item.TransactionMapId,
                        Price = item.Price,
                        Content = item.Content,
                        Bank = item.Bank,
                        AccountNumber = item.AccountNumber,
                    };
                    resultData.Add(transaction);
                }
                transactionResponseDto.TransactionHistories = resultData;
                return new ResponseData<TransactionResponseDto>
                {
                    Success = true,
                    Message = "Successfully Transaction list.",
                    Data = transactionResponseDto,
                    Code = (int)ErrorCodeAPI.OK
                };
            }
            catch (Exception ex)
            {
                return new ResponseData<TransactionResponseDto>
                {
                    Success = false,
                    Message = ex.Message,
                    Code = (int)ErrorCodeAPI.SystemIsError
                };
            }
        }

        public string ConvertContent(string type, Guid id)
        {
            return (type + id.ToString().Replace("-", "")).ToLower();
        }

        public async Task<ResponseData<TransactionQRResponseDto>> GetQR(TransactionQRRequestDto transactionQRRequest)
        {
            try
            {
                TransactionQRResponseDto transactionQR = new TransactionQRResponseDto();
                Transaction transaction = new Transaction();
                transaction.Id = Guid.NewGuid();
                switch (transactionQRRequest.Type)
                {
                    case Constants.TRANS_SERVICE_INVOICE_ALL:
                        if (transactionQRRequest.InvoiceId == null || transactionQRRequest.InvoiceId.Count == 0)
                        {
                            throw new Exception("Ko hop le");
                        }

                        List<Invoice> invoices = await _unitOfWork.InvoiceRepository.GetQuery(x => x.PaidStatus == false && transactionQRRequest.InvoiceId.Contains(x.Id)).ToListAsync();
                        if (invoices == null || invoices.Count == 0)
                        {
                            throw new Exception("Ko tim thay invoice hop le");
                        }
                        transaction.Status = Constants.TRANS_STATUS_LOG_INIT;
                        transaction.Price = 0;
                        transaction.Bank = BankId;
                        transaction.Type = Constants.TRANS_SERVICE_INVOICE_ALL;
                        transaction.AccountNumber = SoTaiKhoan;
                        transaction.Content = ConvertContent(Constants.TRANS_SERVICE_INVOICE_ALL, transaction.Id);
                        transaction.RequestBody = Utill.ConvertObjectToJson(invoices.Select(x => new
                        {
                            x.Id
                        }));
                        foreach (Invoice invoice in invoices)
                        {
                            transaction.Price += invoice.TotalAmount;
                        }

                        TotalAmount = transaction.Price.ToString();
                        NoiDung = transaction.Content;
                        _unitOfWork.TransactionRepository.Add(transaction);
                        await _unitOfWork.SaveChangesAsync();
                        break;
                }
                transactionQR.ImgQR = $"https://img.vietqr.io/image/{BankId}-{SoTaiKhoan}-compact2.jpg?amount={TotalAmount}&addInfo={NoiDung}&accountName={AccountName}";
                transactionQR.Id = transaction.Id;

                return new ResponseData<TransactionQRResponseDto>
                {
                    Success = true,
                    Message = "Successfully Transaction list.",
                    Data = transactionQR,
                    Code = (int)ErrorCodeAPI.OK
                };
            }
            catch (Exception ex)
            {
                return new ResponseData<TransactionQRResponseDto>
                {
                    Success = false,
                    Message = ex.Message,
                    Code = (int)ErrorCodeAPI.SystemIsError
                };
            }
        }
    }
}
