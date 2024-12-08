using AutoMapper;
using CommonModels.Constant;
using Constant;
using Dapper;
using TFU_Building_API.Core.Infrastructure;
using TFU_Building_API.Dto;

namespace TFU_Building_API.Core.Dapper.ServiceContract
{
    public class ServiceContractRepository : IServiceContractRepository
    {
        private readonly IMapper _mapper;
        private readonly DapperCommon _dapperCommon;
        private readonly IConfiguration _configuration;
        private readonly IUserIdentity _userIdentity;

        public ServiceContractRepository(
           IMapper mapper,
           DapperCommon dapperCommon,
           IUserIdentity userIdentity,
           IConfiguration configuration)
        {
            _mapper = mapper;
            _dapperCommon = dapperCommon;
            _configuration = configuration;
            _userIdentity = userIdentity;
        }

        public async Task<IEnumerable<ServiceContractDetailDto>> GetServiceContractDetails(Guid apartmentId)
        {
            DynamicParameters param = new DynamicParameters();
            var query = $"SELECT " +
                $" sc.Id AS ServiceContractId,\n" +
                $"\n    s.ServiceName,   " +
                $"\n  s.Description AS Purpose,   " +
                $"\n  COALESCE(sc.StartDate, GETDATE()) AS StartDate,  " +
                $"\n   CASE    " +
                $"\n     WHEN s.Unit = 'm2' THEN CAST(apt.LandArea AS VARCHAR) + ' m2'       " +
                $"\n   ELSE 'x' + CAST(sc.Quantity AS VARCHAR)    " +
                $"\n  END AS QuantityOrArea, " +
                $"\n    s.UnitPrice,   " +
                $"\n  sc.Status, " +
                $"\n    sc.Note, " +
                $"\n    sc.InsertedAt AS CreatedDate, " +
                $"\n    CASE  " +
                $"\n        WHEN sc.Status IN ('1', '0') THEN sc.UpdatedAt  " +
                $"\n        ELSE NULL  " +
                $"\n    END AS ProcessedDate, " +
                $"\n    sc.InsertedById AS userCreateId " +
                $"\nFROM {DapperConstant.SERVICE_CONTRACTS} sc " +
                $"\nJOIN  " +
                $"\n   {DapperConstant.SERVICE} s ON sc.ServiceId = s.Id " +
                $"\nJOIN " +
                $"\n  {DapperConstant.APARTMENTS} ap ON sc.ApartmentId = ap.Id  " +
                $"\nJOIN  " +
                $"\n  {DapperConstant.APARTMENT_TYPES} apt ON apt.Id = ap.ApartmentTypeId ";

            if (_userIdentity.RoleName.Equals(Constants.ROLE_KI_THUAT))
            {
                query += $"\nJOIN  " +
                    $"{DapperConstant.ASSIGMENTS} ass on ass.ServiceContractId = sc.id  ";
            }

            #region where
            query += $"\nWHERE  " +
            $"\n    sc.IsDeleted = 0 " +
            $"\n    AND s.IsDeleted = 0 " +
            $"\n    AND  sc.ApartmentId = @apartmentId";

            param.Add("@apartmentId", "" + apartmentId + "");

            if (_userIdentity.RoleName.Equals(Constants.ROLE_KI_THUAT))
            {
                query += $"\n AND ass.StaffId = @staffId   ";

                param.Add("@staffId", "" + _userIdentity.UserId + "");
            }
            #endregion

            query += $"\nORDER BY sc.InsertedAt desc";

            //if (!string.IsNullOrEmpty(request.Keyword))
            //{
            //    query += $" AND ToolKey = @keyword ";
            //param.Add("@apartmentId", "" + apartmentId + "");
            //}

            return await _dapperCommon.QueryAsync<ServiceContractDetailDto>(query, param);
        }

        public async Task<IEnumerable<RepairReportServiceDetailDto>> GetRepairReportDetails(Guid serviceContractId)
        {
            DynamicParameters param = new DynamicParameters();
            var query = $"SELECT " +
                $"    sc.Id AS ContractId,\r\n" +
                $"    b.Name AS BuildingName,\r\n" +
                $"    a.RoomNumber AS ApartmentNumber,\r\n" +
                $"    s.ServiceName,\r\n" +
                $"    COALESCE(sc.StartDate, GETDATE()) AS StartTime,\r\n" +
                $"    COALESCE(ass.StartTime, '') AS StartDate,\r\n" +
                $"    COALESCE(sc.EndDate, GETDATE()) AS EndDate,\r\n" +
                $"    ass.EndTime AS StaffEndDate,\r\n" +
                $"    sc.Note,\r\n" +
                $"    sc.NoteDetail,\r\n" +
                $"    sc.NoteFeedbackCuDan,\r\n" +
                $"    sc.NoteKyThuat,\r\n" +
                $"    sc.NoteFeedbackHanhChinh,\r\n" +
                $"    COALESCE(ass.ServicePrice, 0) AS ServicePrice,\r\n" +
                $"  COALESCE(sa.FullName, '') as StaffName,\r\n\t" +
                $"COALESCE(sa.Email, '') as StaffEmail,\r\n\t" +
                $" sa.Id as StaffId," +
                $"    sc.Status";

            #region FROM
            query += $"\nFROM {DapperConstant.SERVICE_CONTRACTS} sc " +
                $"\nJOIN " +
                $"\n  {DapperConstant.APARTMENTS} a ON sc.ApartmentId = a.Id " +
                $"\nJOIN " +
                $"\n  {DapperConstant.SERVICE} s ON sc.ServiceId = s.Id " +
                 $"\n LEFT JOIN " +
                $"\n  {DapperConstant.ASSIGMENTS} ass ON ass.ServiceContractId = sc.Id " +
                $"\nJOIN  " +
                $"\n  {DapperConstant.BUILDINGS} b ON a.BuildingId = b.Id" +
                $"\n LEFT JOIN \r\n\t" +
                $"{DapperConstant.STAFF} sa ON ass.StaffId = sa.Id";

            #endregion

            #region where
            query += $"\nWHERE  " +
            $"\n    sc.IsDeleted = 0 " +
            $"\n    AND s.IsDeleted = 0 " +
            $"\n    AND  sc.Id = @serviceContractId";

            param.Add("@serviceContractId", "" + serviceContractId + "");

            #endregion

            query += $"\nORDER BY sc.InsertedAt desc";

            //if (!string.IsNullOrEmpty(request.Keyword))
            //{
            //    query += $" AND ToolKey = @keyword ";
            //param.Add("@apartmentId", "" + apartmentId + "");
            //}

            return await _dapperCommon.QueryAsync<RepairReportServiceDetailDto>(query, param);
        }
    }
}
