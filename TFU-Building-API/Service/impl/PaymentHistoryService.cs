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
        private string BankId = "970422";
        private string SoTaiKhoan = "1690120007777";
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


            switch (paymentHistoryRequest.Type)
            {
                case Constants.TRANS_SERVICE_INVOICE_ALL:
                    Transaction transaction = _unitOfWork.TransactionRepository.GetById(paymentHistoryRequest.TransactionMapId);
                    if (transaction == null)
                    {
                        return new ResponseData<PaymentHistoryResponseDto>
                        {
                            Success = false,
                            Message = "Khong tim thay thanh toan nay",
                            Data = new PaymentHistoryResponseDto { Result = false },
                            Code = (int)ErrorCodeAPI.InternalError
                        };
                    }
                    //bool check = await CheckBank(content, (decimal)transaction.Price);
                    //if (!check)
                    //{
                    //    return new ResponseData<PaymentHistoryResponseDto>
                    //    {
                    //        Success = false,
                    //        Message = "Khong tim thay thanh toan nay",
                    //        Data = new PaymentHistoryResponseDto { Result = false },
                    //        Code = (int)ErrorCodeAPI.OK
                    //    };
                    //}
                    transaction.Status = Constants.TRANS_STATUS_LOG_DONE;
                    List<RequestBodyItem> items = Utill.ConvertJsonToObject<List<RequestBodyItem>>(transaction.RequestBody);
                    List<Guid> ids = items.Select(item => item.Id).ToList();
                    List<Invoice> invoices = _unitOfWork.InvoiceRepository.GetQuery(x => ids.Contains(x.Id)).ToList();
                    foreach (Invoice invoice in invoices)
                    {
                        invoice.PaidDate = DateTime.Now;
                        invoice.PaidStatus = true;
                        _unitOfWork.InvoiceRepository.Update(invoice);
                    }

                    await _unitOfWork.SaveChangesAsync();
                    return new ResponseData<PaymentHistoryResponseDto>
                    {
                        Success = true,
                        Message = "payment successfully.",
                        Data = new PaymentHistoryResponseDto { Result = true },
                        Code = (int)ErrorCodeAPI.OK
                    };
            }

            return new ResponseData<PaymentHistoryResponseDto>
            {
                Success = true,
                Message = "Khong tim thay thanh toan nay",
                Data = new PaymentHistoryResponseDto { Result = false },
                Code = (int)ErrorCodeAPI.InternalError
            };
        }

        public async Task<bool> CheckBank(string noiDung, decimal price)
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
                //&& !x.Status.Equals(Constants.TRANS_STATUS_LOG_INIT)
                && x.Status.Equals(Constants.TRANS_STATUS_LOG_DONE)
                ).ToListAsync();

                if (transactionRequest.To != null)
                {
                    query = query.Where(x => x.InsertedAt <= transactionRequest.To).ToList();
                }
                if (transactionRequest.From != null)
                {
                    query = query.Where(x => x.InsertedAt >= transactionRequest.To).ToList();
                }

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
                    transactionResponseDto.Pay += transaction.Price;

                }
                transactionResponseDto.Total = transactionResponseDto.Pay + transactionResponseDto.Transfer;
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

        public async Task<ResponseData<UnpaidServiceDetailResponseDto>> GetTransactionsDetail(Guid id)
        {
            try
            {

                TransactionResponseDto transactionResponseDto = new TransactionResponseDto();
                // Stage 1: Fetch data from the database
                var transaction = _unitOfWork.TransactionRepository.GetById(id);

                if (transaction == null)
                {
                    return new ResponseData<UnpaidServiceDetailResponseDto>
                    {
                        Success = false,
                        Message = "Khong tim thay thanh toan nay",
                        Code = (int)ErrorCodeAPI.InternalError
                    };
                }
                List<RequestBodyItem> items = Utill.ConvertJsonToObject<List<RequestBodyItem>>(transaction.RequestBody);
                List<Guid> ids = items.Select(item => item.Id).ToList();

                var query = from inv in _unitOfWork.InvoiceRepository.GetQuery(x => (x.IsDeleted == false))
                            join sc in _unitOfWork.ServiceContractRepository.GetQuery(x => x.IsActive && (x.IsDeleted == false))
                                on inv.ServiceContractId equals sc.Id
                            join s in _unitOfWork.ServiceRepository.GetQuery(x => (x.IsDeleted == false))
                                on sc.ServiceId equals s.Id
                            join a in _unitOfWork.ApartmentRepository.GetQuery(x => (x.IsDeleted == false))
                                on sc.ApartmentId equals a.Id
                            join at in _unitOfWork.ApartmentTypeRepository.GetQuery(x => x.IsActive && (x.IsDeleted == false))
                                on a.ApartmentTypeId equals at.Id
                            join ps in _unitOfWork.PackageServiceRepository.GetQuery(x => x.IsActive && (x.IsDeleted == false))
                                on sc.PackageServiceId equals ps.Id into psJoin
                            from ps in psJoin.DefaultIfEmpty()
                            where inv.IssueDate.HasValue && ids.Contains(inv.Id)
                            select new
                            {
                                InvoiceId = inv.Id,
                                ServiceName = s.ServiceName,
                                Description = s.Description,
                                QuantityOrArea = s.Unit == "m2" ? $"{at.LandArea} m2" : $"x{sc.Quantity}",
                                UnitPrice = s.UnitPrice,
                                Discount = ps.Discount ?? 0,
                                Unit = s.Unit,
                                LandArea = at.LandArea,
                                Quantity = sc.Quantity,
                                StartDate = sc.StartDate ?? DateTime.Now,
                                EndDate = sc.EndDate ?? DateTime.Now,
                                PaidStatus = inv.PaidStatus,
                                PaymentDate = inv.UpdatedAt
                            };




                var result = await query.ToListAsync();

                // Tính toán TotalPrice sau khi dữ liệu được tải
                var services = result.Select(item => new UnpaidServiceDetailDto
                {
                    InvoiceId = item.InvoiceId,
                    ServiceName = item.ServiceName,
                    Description = item.Description,
                    QuantityOrArea = item.QuantityOrArea,
                    UnitPrice = item.UnitPrice,
                    TotalPrice = Utill.CalculateTotalPrice(item.UnitPrice, item.StartDate, item.EndDate, item.Discount, item.Unit, item.LandArea, item.Quantity),
                    PaymentStatus = item.PaidStatus ? "Đã thanh toán" : "Chưa thanh toán",
                    PaymentDate = item.PaidStatus ? item.PaymentDate : null // Lấy PaymentDate nếu đã thanh toán
                }).ToList();

                // Sắp xếp để dịch vụ mặc định ("Dịch vụ phòng") lên đầu
                services = services
                    .OrderByDescending(s => s.ServiceName == "Dịch vụ phòng") // Đưa "Dịch vụ phòng" lên đầu
                    .ThenBy(s => s.ServiceName) // Sắp xếp các dịch vụ còn lại theo tên (tùy chọn)
                    .ToList();

                // Tính tổng giá của tất cả dịch vụ
                var totalAmount = services.Sum(x => x.TotalPrice);

                // Đóng gói kết quả vào UnpaidServiceDetailResponseDto
                var response = new UnpaidServiceDetailResponseDto
                {
                    Services = services,
                    TotalAmount = totalAmount
                };

                return new ResponseData<UnpaidServiceDetailResponseDto>
                {
                    Success = true,
                    Message = "Successfully retrieved service details.",
                    Data = response,
                    Code = (int)ErrorCodeAPI.OK
                };
            }
            catch (Exception ex)
            {
                return new ResponseData<UnpaidServiceDetailResponseDto>
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
                        }).ToList());
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
