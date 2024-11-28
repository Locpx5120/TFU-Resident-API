using BuildingModels;
using Core.Enums;
using Core.Model;
using Microsoft.EntityFrameworkCore;
using QRCoder;
using System.Linq;
using TFU_Building_API.Core.Handler;
using TFU_Building_API.Core.Helper;
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

        //public async Task<ResponseData<string>> AddInvoicesForApartment(CreateInvoiceRequestDto request)
        //{
        //    try
        //    {
        //        var currentMonth = DateTime.Now.Month;
        //        var currentYear = DateTime.Now.Year;

        //        // Xác định IssueDate và DueDate
        //        var issueDate = new DateTime(currentYear, currentMonth, 1);
        //        var dueDate = new DateTime(currentYear, currentMonth, 10);

        //        // Lấy UserId của chủ sở hữu căn hộ từ bảng Ownership
        //        var ownership = await _unitOfWork.OwnerShipRepository.GetQuery(x => x.ApartmentId == request.ApartmentId && (x.IsDeleted == false))
        //                                                             .FirstOrDefaultAsync();

        //        if (ownership == null)
        //        {
        //            return new ResponseData<string>
        //            {
        //                Success = false,
        //                Message = "Ownership information not found for this apartment.",
        //                Code = (int)ErrorCodeAPI.NotFound
        //            };
        //        }

        //        var userId = ownership.ResidentId;

        //        // Kiểm tra xem đã có Invoice tồn tại cho các dịch vụ của căn hộ trong tháng hiện tại không
        //        var existingInvoices = await _unitOfWork.InvoiceRepository.GetQuery(x => x.ResidentId == userId && x.IssueDate.HasValue
        //                                                                              && x.IssueDate.Value.Month == currentMonth
        //                                                                              && x.IssueDate.Value.Year == currentYear
        //                                                                              && (x.IsDeleted == false))
        //                                        .Select(x => x.ServiceContractId)
        //                                        .ToListAsync();

        //        // Lấy tất cả các hợp đồng dịch vụ cho căn hộ, bao gồm Service, Apartment và ApartmentType
        //        var serviceContracts = await _unitOfWork.ServiceContractRepository
        //            .GetQuery(x => x.ApartmentId == request.ApartmentId && x.Status == ServiceContractStatus.Approved  && x.IsActive && (x.IsDeleted == false))
        //            .Include(sc => sc.Service) // Eager-load bảng Service
        //            .Include(sc => sc.Apartment) // Eager-load bảng Apartment
        //            .ThenInclude(a => a.ApartmentType) // Eager-load bảng ApartmentType thông qua Apartment
        //            .Include(sc => sc.PackageService) // Eager-load bảng PackageService
        //            .ToListAsync();

        //        // Lọc ra các hợp đồng dịch vụ chưa có hóa đơn trong tháng
        //        var newServiceContracts = serviceContracts.Where(sc => !existingInvoices.Contains(sc.Id)).ToList();

        //        foreach (var serviceContract in newServiceContracts)
        //        {
        //            // Lấy UnitPrice và chiết khấu
        //            decimal baseUnitPrice = serviceContract.Service.UnitPrice;
        //            decimal discount = serviceContract.PackageService?.Discount ?? 0;

        //            decimal totalAmount;

        //            if (serviceContract.Service.Unit == "m2" && serviceContract.Apartment != null && serviceContract.Apartment.ApartmentType != null)
        //            {
        //                // Tính số tháng giữa StartDate và EndDate
        //                int months = ((serviceContract.EndDate.Value.Year - serviceContract.StartDate.Value.Year) * 12)
        //                             + serviceContract.EndDate.Value.Month - serviceContract.StartDate.Value.Month + 1;

        //                // Tính giá cho mỗi tháng và áp dụng chiết khấu cho từng tháng
        //                //decimal amountPerMonth = baseUnitPrice * serviceContract.Apartment.ApartmentType.LandArea * (1 - discount / 100);

        //                decimal amountPerMonth = baseUnitPrice;
        //                totalAmount = amountPerMonth * months;
        //            }
        //            else
        //            {
        //                // Tính số ngày giữa StartDate và EndDate
        //                int days = (serviceContract.EndDate.Value - serviceContract.StartDate.Value).Days;

        //                // Tính giá cơ bản cho từng ngày dựa trên Quantity và áp dụng chiết khấu cho từng ngày
        //                decimal amountPerDay = baseUnitPrice * (serviceContract.Quantity ?? 0) * (1 - discount / 100);
        //                totalAmount = amountPerDay * days;
        //            }

        //            var invoice = new Invoice
        //            {
        //                Id = Guid.NewGuid(),
        //                IssueDate = issueDate,
        //                DueDate = dueDate,
        //                PaidStatus = false,
        //                ServiceContractId = serviceContract.Id,
        //                TotalAmount = totalAmount,
        //                ResidentId = userId ?? new Guid(),
        //                IsDeleted = false,
        //                InsertedAt = DateTime.Now,
        //                UpdatedAt = DateTime.Now,
        //                IsActive = true
        //            };

        //            _unitOfWork.InvoiceRepository.Add(invoice);
        //        }

        //        // Lưu thay đổi
        //        await _unitOfWork.SaveChangesAsync();

        //        return new ResponseData<string>
        //        {
        //            Success = true,
        //            Message = newServiceContracts.Any()
        //                ? "Invoices created successfully for new services of this apartment."
        //                : "All services of this apartment already have invoices for the current month.",
        //            Code = (int)ErrorCodeAPI.OK
        //        };
        //    }
        //    catch (Exception ex)
        //    {
        //        return new ResponseData<string>
        //        {
        //            Success = false,
        //            Message = ex.Message,
        //            Code = (int)ErrorCodeAPI.SystemIsError
        //        };
        //    }
        //}

        public async Task<ResponseData<string>> AddInvoicesForApartment(CreateInvoiceRequestDto request)
        {
            try
            {
                var currentMonth = DateTime.Now.Month;
                var currentYear = DateTime.Now.Year;

                // Xác định IssueDate và DueDate
                var issueDate = new DateTime(currentYear, currentMonth, 1);
                var dueDate = new DateTime(currentYear, currentMonth, 10);

                // Lấy danh sách các căn hộ mà User đang sở hữu
                var ownerships = await _unitOfWork.OwnerShipRepository
                    .GetQuery(x => x.ResidentId == request.UserId && x.IsDeleted == false)
                    .ToListAsync();

                if (!ownerships.Any())
                {
                    return new ResponseData<string>
                    {
                        Success = false,
                        Message = "This user does not own any apartments.",
                        Code = (int)ErrorCodeAPI.NotFound
                    };
                }

                // Lấy tất cả các ApartmentId mà User sở hữu
                var apartmentIds = ownerships.Select(o => o.ApartmentId).ToList();

                // Lấy danh sách các ServiceContract liên quan tới các ApartmentId
                var serviceContracts = await _unitOfWork.ServiceContractRepository
                    .GetQuery(x => apartmentIds.Contains(x.ApartmentId) && x.Status == ServiceContractStatus.Approved && x.IsActive && x.IsDeleted == false)
                    .Include(sc => sc.Service) // Eager-load bảng Service
                    .Include(sc => sc.Apartment) // Eager-load bảng Apartment
                    .ThenInclude(a => a.ApartmentType) // Eager-load bảng ApartmentType
                    .Include(sc => sc.PackageService) // Eager-load bảng PackageService
                    .ToListAsync();

                // Kiểm tra xem đã có hóa đơn nào tồn tại trong tháng hiện tại cho các hợp đồng dịch vụ này không
                var existingInvoices = await _unitOfWork.InvoiceRepository
                    .GetQuery(x => x.ResidentId == request.UserId && x.IssueDate.HasValue && x.IssueDate.Value.Month == currentMonth && x.IssueDate.Value.Year == currentYear && x.IsDeleted == false)
                    .Select(x => x.ServiceContractId)
                    .ToListAsync();

                // Tạo HashSet cho các hóa đơn đã tồn tại để tối ưu việc tra cứu
                var existingInvoiceSet = new HashSet<Guid>(existingInvoices);

                // Các GUID của các ServiceId không cần thiết
                var excludedServiceIds = new HashSet<Guid>
                {
                    new Guid("F517BEF7-D325-487B-9F76-EB5D20413634"),
                    new Guid("F517BEF7-D325-487B-9F76-E66D20413634")
                };

                // Lọc các ServiceContract chưa có hóa đơn trong tháng và loại các dịch vụ không cần thiết
                var newServiceContracts = serviceContracts
                    .Where(sc => !existingInvoiceSet.Contains(sc.Id) && !excludedServiceIds.Contains((Guid)sc.ServiceId))
                    .ToList();


                // Tạo hóa đơn cho các hợp đồng dịch vụ chưa có hóa đơn
                foreach (var serviceContract in newServiceContracts)
                {
                    decimal baseUnitPrice = serviceContract.Service.UnitPrice;
                    decimal discount = serviceContract.PackageService?.Discount ?? 0;

                    decimal totalAmount;

                    if (serviceContract.Service.Unit == "m2" && serviceContract.Apartment != null && serviceContract.Apartment.ApartmentType != null)
                    {
                        int months = ((serviceContract.EndDate.Value.Year - serviceContract.StartDate.Value.Year) * 12) +
                                     serviceContract.EndDate.Value.Month - serviceContract.StartDate.Value.Month + 1;

                        // Tính giá dịch vụ phòng mặc định
                        totalAmount = baseUnitPrice * months;
                    }
                    else
                    {
                        int days = (serviceContract.EndDate.Value - serviceContract.StartDate.Value).Days;

                        // Tính giá cơ bản cho mỗi ngày và áp dụng chiết khấu
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
                        ResidentId = request.UserId,
                        IsDeleted = false,
                        InsertedAt = DateTime.Now,
                        UpdatedAt = DateTime.Now,
                        IsActive = true
                    };

                    _unitOfWork.InvoiceRepository.Add(invoice);
                }

                // Lưu các thay đổi vào cơ sở dữ liệu
                await _unitOfWork.SaveChangesAsync();

                return new ResponseData<string>
                {
                    Success = true,
                    Message = newServiceContracts.Any()
                        ? "Invoices created successfully for new services of all apartments owned by this user."
                        : "All services of this user already have invoices for the current month.",
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
                foreach (var invoiceId in request.InvoiceIds)
                {
                    // Tìm invoice dựa vào từng InvoiceId
                    var invoice = await _unitOfWork.InvoiceRepository.GetByIdAsync(invoiceId);
                    if (invoice == null || invoice.IsDeleted == true)
                    {
                        return new ResponseData<InvoicePaymentResponseDto>
                        {
                            Success = false,
                            Message = $"Invoice with ID {invoiceId} not found.",
                            Code = (int)ErrorCodeAPI.NotFound
                        };
                    }

                    // Cập nhật thông tin thanh toán
                    invoice.PaidStatus = true;
                    invoice.PaidDate = DateTime.Now;
                    invoice.UpdatedAt = DateTime.Now;

                    _unitOfWork.InvoiceRepository.Update(invoice);
                }

                // Lưu tất cả thay đổi một lần
                await _unitOfWork.SaveChangesAsync();

                // Tạo QR Code
                var qrCodeData = GenerateQRCode(request.BankAccountName, request.BankAccountNumber, request.BankName, request.Amount, request.TransactionContent);
                var qrCodeUrl = $"data:image/png;base64,{Convert.ToBase64String(qrCodeData)}";

                return new ResponseData<InvoicePaymentResponseDto>
                {
                    Success = true,
                    Message = "All payments processed successfully.",
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
                    Code = (int)ErrorCodeAPI.SystemIsError,
                    Data = new InvoicePaymentResponseDto
                    {
                        Success = false,
                        Message = ex.Message
                    }
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

        public async Task<ResponseData<List<ResidentPaymentInfoDto>>> GetResidentPaymentListAsync(PaymentFilterDto filter)
        {
            try
            {
                var query = from invoice in _unitOfWork.InvoiceRepository.GetQuery(i => i.IsDeleted == false)
                            join resident in _unitOfWork.ResidentRepository.GetQuery(r => r.IsDeleted == false) on invoice.ResidentId equals resident.Id
                            join apartment in _unitOfWork.ApartmentRepository.GetQuery(a => a.IsDeleted == false) on invoice.ServiceContract.ApartmentId equals apartment.Id
                            join building in _unitOfWork.BuildingRepository.GetQuery(b => b.IsDeleted == false) on apartment.BuildingId equals building.Id
                            select new ResidentPaymentInfoDto
                            {
                                BuildingId = building.Id,
                                ResidentName = resident.Name,
                                BuildingName = building.Name,
                                RoomNumber = apartment.RoomNumber,
                                TotalAmount = invoice.TotalAmount,
                                PaymentStatus = invoice.PaidStatus ? "Đã trả" : "Chưa trả",
                                PaymentDate = invoice.PaidDate,
                                InvoiceId = invoice.Id,
                                PaidStatus = invoice.PaidStatus,
                            };

                // Apply filters
                if (!string.IsNullOrEmpty(filter.ResidentName))
                {
                    query = query.Where(q => EF.Functions.Like(q.ResidentName, $"%{filter.ResidentName}%"));
                }
                if (filter.BuildingId.HasValue)
                {
                    query = query.Where(q => q.BuildingId == filter.BuildingId.Value);
                }
                if (filter.IsPaid.HasValue)
                {
                    query = query.Where(q => q.PaidStatus == filter.IsPaid.Value);
                }
                if (filter.PaymentMonth.HasValue)
                {
                    query = query.Where(q => q.PaymentDate.HasValue && q.PaymentDate.Value.Month == filter.PaymentMonth.Value.Month && q.PaymentDate.Value.Year == filter.PaymentMonth.Value.Year);
                }

                var result = await query.ToListAsync();

                return new ResponseData<List<ResidentPaymentInfoDto>>
                {
                    Success = true,
                    Message = "Payment information retrieved successfully.",
                    Data = result,
                    Code = (int)ErrorCodeAPI.OK
                };
            }
            catch (Exception ex)
            {
                return new ResponseData<List<ResidentPaymentInfoDto>>
                {
                    Success = false,
                    Message = ex.Message,
                    Code = (int)ErrorCodeAPI.SystemIsError
                };
            }
        }

    }
}
