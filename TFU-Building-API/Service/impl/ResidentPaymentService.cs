using Core.Enums;
using Core.Model;
using Microsoft.EntityFrameworkCore;
using TFU_Building_API.Core.Handler;
using TFU_Building_API.Core.Infrastructure;
using TFU_Building_API.Dto;

namespace TFU_Building_API.Service.impl
{
    public class ResidentPaymentService : BaseHandler, IResidentPayment
    {
        private readonly IUnitOfWork _unitOfWork;
        public ResidentPaymentService(IUnitOfWork UnitOfWork, IHttpContextAccessor HttpContextAccessor) : base(UnitOfWork, HttpContextAccessor)
        {
            _unitOfWork = UnitOfWork;
        }

        public async Task<ResponseData<PaginatedResponseDto<ResidentPaymentDto>>> GetResidentPayments(ResidentPaymentRequestDto request)
        {
            try
            {
                var currentMonth = request.Month ?? DateTime.Now.Month;
                var currentYear = request.Year ?? DateTime.Now.Year;

                // Lấy dữ liệu thô từ bảng với các bộ lọc
                var query = from sc in _unitOfWork.ServiceContractRepository.GetQuery(x => x.IsActive && x.IsDeleted == false)
                            join own in _unitOfWork.OwnerShipRepository.GetQuery(x => x.IsDeleted == false)
                                on sc.ApartmentId equals own.ApartmentId
                            join apt in _unitOfWork.ApartmentRepository.GetQuery(x => x.IsDeleted == false)
                                on own.ApartmentId equals apt.Id
                            join svc in _unitOfWork.ServiceRepository.GetQuery(x => x.IsDeleted == false)
                                on sc.ServiceId equals svc.Id
                            join inv in _unitOfWork.InvoiceRepository.GetQuery(x => x.IsDeleted == false && x.IssueDate.HasValue
                                && x.IssueDate.Value.Month == currentMonth && x.IssueDate.Value.Year == currentYear)
                                on sc.Id equals inv.ServiceContractId
                            select new
                            {
                                ApartmentId = apt.Id,
                                OwnerName = own.Resident.Name,
                                Floor = apt.FloorNumber,
                                RoomNumber = apt.RoomNumber,
                                UnitPrice = svc.UnitPrice,
                                Unit = svc.Unit,
                                Quantity = sc.Quantity,
                                LandArea = apt.ApartmentType.LandArea,
                                Discount = sc.PackageService.Discount,
                                PaidStatus = inv.PaidStatus
                            };

                var rawData = await query.ToListAsync();

                // Xử lý tính toán TotalAmount và IsPaid bên ngoài truy vấn
                var groupedData = rawData.GroupBy(x => new { x.ApartmentId, x.OwnerName, x.Floor, x.RoomNumber })
                    .Select(g => new ResidentPaymentDto
                    {
                        ApartmentId = g.Key.ApartmentId,
                        OwnerName = g.Key.OwnerName,
                        Floor = g.Key.Floor,
                        RoomNumber = g.Key.RoomNumber,
                        TotalAmount = g.Sum(x =>
                        {
                            decimal baseUnitPrice = x.UnitPrice;
                            decimal quantityOrArea = x.Quantity ?? 1;

                            // Tính số tiền dựa trên đơn vị (m2 hoặc số lượng)
                            if (x.Unit == "m2")
                            {
                                quantityOrArea = x.LandArea;
                            }

                            // Tính chiết khấu
                            decimal discount = x.Discount ?? 0;
                            decimal unitPriceWithDiscount = baseUnitPrice * (1 - discount / 100);

                            // Tổng giá trị dịch vụ
                            return unitPriceWithDiscount * quantityOrArea;
                        }),
                        IsPaid = g.All(x => x.PaidStatus) // Kiểm tra nếu tất cả các hóa đơn đều đã thanh toán
                    }).ToList();

                // Áp dụng phân trang
                var totalRecords = groupedData.Count;
                var pagedData = groupedData
                    .Skip((request.PageNumber - 1) * request.PageSize)
                    .Take(request.PageSize)
                    .ToList();

                var paginatedResponse = new PaginatedResponseDto<ResidentPaymentDto>
                {
                    TotalRecords = totalRecords,
                    Data = pagedData
                };

                return new ResponseData<PaginatedResponseDto<ResidentPaymentDto>>
                {
                    Success = true,
                    Message = "Successfully retrieved resident payments.",
                    Data = paginatedResponse,
                    Code = (int)ErrorCodeAPI.OK
                };
            }
            catch (Exception ex)
            {
                return new ResponseData<PaginatedResponseDto<ResidentPaymentDto>>
                {
                    Success = false,
                    Message = ex.Message,
                    Code = (int)ErrorCodeAPI.SystemIsError
                };
            }
        }



    }
}
