using BuildingModels;
using Core.Enums;
using Core.Model;
using Microsoft.EntityFrameworkCore;
using QRCoder;
using TFU_Building_API.Core.Handler;
using TFU_Building_API.Core.Infrastructure;
using TFU_Building_API.Dto;

namespace TFU_Building_API.Service.impl
{
    public class InvoiceService : BaseHandler, IInvoiceService
    {
        private readonly IUnitOfWork _unitOfWork;

        public InvoiceService(IUnitOfWork UnitOfWork, IHttpContextAccessor HttpContextAccessor) : base(UnitOfWork, HttpContextAccessor)
        {
            _unitOfWork = UnitOfWork;
        }

        public async Task<ResponseData<string>> AddInvoicesForApartment(CreateInvoiceRequestDto request)
        {
            try
            {
                var currentMonth = DateTime.Now.Month;
                var currentYear = DateTime.Now.Year;

                // Xác định IssueDate và DueDate
                var issueDate = new DateTime(currentYear, currentMonth, 1);
                var dueDate = new DateTime(currentYear, currentMonth, 10);

                // Lấy UserId của chủ sở hữu căn hộ từ bảng Ownership
                var ownership = await _unitOfWork.OwnerShipRepository.GetQuery(x => x.ApartmentId == request.ApartmentId && (x.IsDeleted == false))
                                                                     .FirstOrDefaultAsync();

                if (ownership == null)
                {
                    return new ResponseData<string>
                    {
                        Success = false,
                        Message = "Ownership information not found for this apartment.",
                        Code = (int)ErrorCodeAPI.NotFound
                    };
                }

                var userId = ownership.ResidentId;

                // Kiểm tra xem đã có Invoice tồn tại cho các dịch vụ của căn hộ trong tháng hiện tại không
                var existingInvoices = await _unitOfWork.InvoiceRepository.GetQuery(x => x.ResidentId == userId && x.IssueDate.HasValue
                                                                                      && x.IssueDate.Value.Month == currentMonth
                                                                                      && x.IssueDate.Value.Year == currentYear
                                                                                      && (x.IsDeleted == false))
                                                .Select(x => x.ServiceContractId)
                                                .ToListAsync();

                // Lấy tất cả các hợp đồng dịch vụ cho căn hộ, bao gồm Service, Apartment và ApartmentType
                var serviceContracts = await _unitOfWork.ServiceContractRepository
                    .GetQuery(x => x.ApartmentId == request.ApartmentId && x.IsActive && (x.IsDeleted == false))
                    .Include(sc => sc.Service) // Eager-load bảng Service
                    .Include(sc => sc.Apartment) // Eager-load bảng Apartment
                    .ThenInclude(a => a.ApartmentType) // Eager-load bảng ApartmentType thông qua Apartment
                    .Include(sc => sc.PackageService) // Eager-load bảng PackageService
                    .ToListAsync();

                // Lọc ra các hợp đồng dịch vụ chưa có hóa đơn trong tháng
                var newServiceContracts = serviceContracts.Where(sc => !existingInvoices.Contains(sc.Id)).ToList();

                foreach (var serviceContract in newServiceContracts)
                {
                    // Lấy UnitPrice và chiết khấu
                    decimal baseUnitPrice = serviceContract.Service.UnitPrice;
                    decimal discount = serviceContract.PackageService?.Discount ?? 0;

                    decimal totalAmount;

                    if (serviceContract.Service.Unit == "m2" && serviceContract.Apartment != null && serviceContract.Apartment.ApartmentType != null)
                    {
                        // Tính số tháng giữa StartDate và EndDate
                        int months = ((serviceContract.EndDate.Value.Year - serviceContract.StartDate.Value.Year) * 12)
                                     + serviceContract.EndDate.Value.Month - serviceContract.StartDate.Value.Month + 1;

                        // Tính giá cho mỗi tháng và áp dụng chiết khấu cho từng tháng
                        //decimal amountPerMonth = baseUnitPrice * serviceContract.Apartment.ApartmentType.LandArea * (1 - discount / 100);

                        decimal amountPerMonth = baseUnitPrice;
                        totalAmount = amountPerMonth * months;
                    }
                    else
                    {
                        // Tính số ngày giữa StartDate và EndDate
                        int days = (serviceContract.EndDate.Value - serviceContract.StartDate.Value).Days;

                        // Tính giá cơ bản cho từng ngày dựa trên Quantity và áp dụng chiết khấu cho từng ngày
                        decimal amountPerDay = baseUnitPrice * (serviceContract.Quantity ?? 0) * (1 - discount / 100);
                        totalAmount = amountPerDay * days;
                    }

                    var invoice = new Invoice
                    {
                        Id = Guid.NewGuid(),
                        IssueDate = issueDate,
                        DueDate = dueDate,
                        PaidStatus = false,
                        ServiceContractId = serviceContract.Id,
                        TotalAmount = totalAmount,
                        ResidentId = userId ?? new Guid(),
                        IsDeleted = false,
                        InsertedAt = DateTime.Now,
                        UpdatedAt = DateTime.Now,
                        IsActive = true
                    };

                    _unitOfWork.InvoiceRepository.Add(invoice);
                }

                // Lưu thay đổi
                await _unitOfWork.SaveChangesAsync();

                return new ResponseData<string>
                {
                    Success = true,
                    Message = newServiceContracts.Any()
                        ? "Invoices created successfully for new services of this apartment."
                        : "All services of this apartment already have invoices for the current month.",
                    Code = (int)ErrorCodeAPI.OK
                };
            }
            catch (Exception ex)
            {
                return new ResponseData<string>
                {
                    Success = false,
                    Message = ex.Message,
                    Code = (int)ErrorCodeAPI.SystemIsError
                };
            }
        }

        public async Task<ResponseData<InvoicePaymentResponseDto>> ProcessInvoicePaymentAsync(InvoicePaymentRequestDto request)
        {
            try
            {
                // Tìm invoice dựa vào InvoiceId
                var invoice = await _unitOfWork.InvoiceRepository.GetByIdAsync(request.InvoiceId);
                if (invoice == null || invoice.IsDeleted ==  true)
                {
                    return new ResponseData<InvoicePaymentResponseDto>
                    {
                        Success = false,
                        Message = "Invoice not found.",
                        Code = (int)ErrorCodeAPI.NotFound
                    };
                }

                // Cập nhật thông tin thanh toán
                invoice.PaidStatus = true;
                invoice.PaidDate = DateTime.Now;
                invoice.UpdatedAt = DateTime.Now;

                _unitOfWork.InvoiceRepository.Update(invoice);
                await _unitOfWork.SaveChangesAsync();

                // Tạo QR Code
                var qrCodeData = GenerateQRCode(request.BankAccountName, request.BankAccountNumber, request.BankName, request.Amount, request.TransactionContent);
                var qrCodeUrl = $"data:image/png;base64,{Convert.ToBase64String(qrCodeData)}";

                return new ResponseData<InvoicePaymentResponseDto>
                {
                    Success = true,
                    Message = "Payment processed successfully.",
                    Data = new InvoicePaymentResponseDto
                    {
                        Success = true,
                        QRCodeUrl = qrCodeUrl,
                        Message = "Please scan the QR code with your banking app to proceed with payment."
                    },
                    Code = (int)ErrorCodeAPI.OK
                };
            }
            catch (Exception ex)
            {
                return new ResponseData<InvoicePaymentResponseDto>
                {
                    Success = false,
                    Message = ex.Message,
                    Code = (int)ErrorCodeAPI.SystemIsError
                };
            }
        }

        private byte[] GenerateQRCode(string accountName, string accountNumber, string bankName, decimal amount, string transactionContent)
        {
            using (var qrGenerator = new QRCodeGenerator())
            {
                string qrContent = $"BankAccountName={accountName}&BankAccountNumber={accountNumber}&BankName={bankName}&Amount={amount}&TransactionContent={transactionContent}";
                var qrCodeData = qrGenerator.CreateQrCode(qrContent, QRCodeGenerator.ECCLevel.Q);

                // Use the fully qualified name here to avoid conflicts
                using (var qrCode = new QRCoder.QRCode(qrCodeData))
                using (var bitmap = qrCode.GetGraphic(20))
                using (var stream = new MemoryStream())
                {
                    bitmap.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
                    return stream.ToArray();
                }
            }
        }

    }
}
