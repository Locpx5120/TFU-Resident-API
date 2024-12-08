using AutoMapper;
using CommonModels.Constant;
using Dapper;
using TFU_Building_API.Core.Infrastructure;
using TFU_Building_API.Dto;

namespace TFU_Building_API.Core.Dapper.User
{
    public class UserRepository : IUserRepository
    {
        private readonly IMapper _mapper;
        private readonly DapperCommon _dapperCommon;
        private readonly IConfiguration _configuration;
        private readonly IUserIdentity _userIdentity;

        public UserRepository(
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

        public async Task<IEnumerable<GetStaffAssigmentResponseDto>> GetStaffListAssigment(string searchName)
        {
            if (String.IsNullOrEmpty(searchName) || String.IsNullOrEmpty(searchName.Trim()))
            {
                searchName = String.Empty;
            }
            DynamicParameters param = new DynamicParameters();
            var query = $"select s.id, s.FullName, s.Email, s.PhoneNumber\r\n" +
                $"from {DapperConstant.STAFF} s\r\n" +
                $"where id in (\r\n\t\t\t" +
                $"select distinct(s.id)\r\n\t\t\t" +
                $"from {DapperConstant.STAFF} s" +
                $" left join {DapperConstant.ASSIGMENTS} ass on s.Id = ass.StaffId \r\n\t\t\t" +
                $" join {DapperConstant.SERVICE_CONTRACTS} se on se.id = ass.ServiceContractId " +
                $"where s.RoleId = '{DapperConstant.ID_ROLE_KY_THUAT}' and s.IsActive = 1 " +
                $"and (" +
                $"  (ass.StartTime is null or ass.EndTime is not null) or (ass.StartTime is not null and ass.EndTime is not null)" +
                $" or se.Status = 1 " +
                $" )\r\n\t\t" +
                $")\r\n\t" +
                $"and s.Email like @keyword ";

            param.Add("@keyword", "" + searchName + "%");
            return await _dapperCommon.QueryAsync<GetStaffAssigmentResponseDto>(query, param);
        }
    }
}
