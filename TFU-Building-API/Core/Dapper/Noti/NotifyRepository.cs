using AutoMapper;
using Dapper;
using TFU_Building_API.Core.Infrastructure;
using TFU_Building_API.Dto;

namespace TFU_Building_API.Core.Dapper.Noti
{
    public class NotifyRepository : INotifyRepository
    {
        private readonly IMapper _mapper;
        private readonly DapperCommon _dapperCommon;
        private readonly IConfiguration _configuration;
        private readonly IUserIdentity _userIdentity;

        public NotifyRepository(
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

        public async Task<IEnumerable<NotifyResponseDto>> GetNotifies(NotifyFilterRequestDto request)
        {
            DynamicParameters param = new DynamicParameters();
            var query = $"SELECT  n.id," +
                $" b.Name as BuildingName," +
                $" n.NotificationType," +
                $" n.Title," +
                $" COALESCE(r.Name, 'All')  as RoleName" +
                $"\r\n\t\t, n.ApplyDate as Date" +
                $"\r\n\t\t, ( COALESCE(re.Email, '') + '' + COALESCE(sa.Email, '') ) as CreatedBy" +
                $"\r\n\t\t, ( COALESCE(reUpdate.Email, '') + '' + COALESCE(saUpdate.Email, '') ) as ApprovedBy" +
                $"\r\n\t\t, n.Status" +
                $"\r\n\t\t, n.ImgBaseId " +
                $"\r\n\t\t, n.BuildingId " +
                $"\r\n\t\t, n.ShortContent as ShortContent " +

                $"\r\n  FROM Notifies n " +
                $"\r\n  join Buildings b on n.BuildingId = b.id " +
                $"\r\n  left join Roles r on r.Id = n.RoleId" +
                $"\r\n  left join Residents re on re.Id = n.InsertedById  " +
                $"\r\n  left join Staff sa on sa.Id = n.InsertedById" +
                $"\r\n  left join Residents reUpdate on reUpdate .Id = n.UserAccpectId  " +
                $"\r\n  left join Staff saUpdate on saUpdate.Id = n.UserAccpectId";

            #region Where
            query += $"\r\n\tWhere n.Title like @title";
            param.Add("@title", "%" + request.Title + "%");
            if (!String.IsNullOrEmpty(request.NotificationType))
            {
                query += $" AND  n.NotificationType = @notificationType ";
                param.Add("@notificationType", "" + request.NotificationType + "");
            }
            if (request.ApplyDate != null)
            {
                query += $"\r\n\tand n.ApplyDate >= @applyDateTo " +
                $" and n.ApplyDate < @applyDateFrom ";

                param.Add("@applyDateTo", "" + request.ApplyDate + "");
                param.Add("@applyDateFrom", "" + request.ApplyDate.Value.AddDays(1) + "");
            }
            if (!String.IsNullOrEmpty(request.Status))
            {
                query += $"\r\n\tand n.Status = @status ";
                param.Add("@status", "" + request.Status + "");
            }
            #endregion

            #region Order by
            query += $"\r\n\r\n  Order by n.InsertedAt desc ";

            #endregion

            return await _dapperCommon.QueryAsync<NotifyResponseDto>(query, param);
        }

        public async Task<IEnumerable<NotifyResponseDto>> GetNotifiesByUser()
        {
            DynamicParameters param = new DynamicParameters();
            var query = $"SELECT  n.id," +
                $" b.Name as BuildingName," +
                $" n.NotificationType," +
                $" n.Title," +
                $" COALESCE(r.Name, 'All')  as RoleName" +
                $"\r\n\t\t, n.ApplyDate as Date" +
                $"\r\n\t\t, ( COALESCE(re.Email, '') + '' + COALESCE(sa.Email, '') ) as CreatedBy" +
                $"\r\n\t\t, ( COALESCE(reUpdate.Email, '') + '' + COALESCE(saUpdate.Email, '') ) as ApprovedBy" +
                $"\r\n\t\t, n.Status" +
                $"\r\n\t\t, n.ImgBaseId " +
                $"\r\n\t\t, n.BuildingId " +
                $"\r\n\t\t, n.ShortContent as ShortContent " +
                $"\r\n\t\t, n.LongContent  " +

                $"\r\n  FROM Notifies n " +
                $"\r\n  join Buildings b on n.BuildingId = b.id " +
                $"\r\n  left join Roles r on r.Id = n.RoleId" +
                $"\r\n  left join Residents re on re.Id = n.InsertedById  " +
                $"\r\n  left join Staff sa on sa.Id = n.InsertedById" +
                $"\r\n  left join Residents reUpdate on reUpdate .Id = n.UserAccpectId  " +
                $"\r\n  left join Staff saUpdate on saUpdate.Id = n.UserAccpectId";

            #region Where
            query += $"\r\n\tWhere r.Name is null or r.Name_En = @name_En";
            param.Add("@name_En", "" + _userIdentity.RoleName + "");
            query += $"\r\n\t AND n.Status = 'APPLYING'";
            #endregion

            #region Order by
            query += $"\r\n\r\n  Order by n.InsertedAt desc ";

            #endregion

            return await _dapperCommon.QueryAsync<NotifyResponseDto>(query, param);
        }

        public async Task<IEnumerable<NotifyDetailResponseDto>> GetNotifiesDetails(Guid notifyId)
        {
            DynamicParameters param = new DynamicParameters();
            var query = $"SELECT  n.id," +
                $" b.Name as BuildingName," +
                $" n.NotificationType," +
                $" n.Title," +
                $" COALESCE(r.Name, 'All')  as RoleName" +
                $"\r\n\t\t, n.ApplyDate as Date" +
                $"\r\n\t\t, ( COALESCE(re.Email, '') + '' + COALESCE(sa.Email, '') ) as CreatedBy" +
                $"\r\n\t\t, ( COALESCE(reUpdate.Email, '') + '' + COALESCE(saUpdate.Email, '') ) as ApprovedBy" +
                $"\r\n\t\t, n.Status" +
                $"\r\n\t\t, n.ImgBaseId " +
                $"\r\n\t\t, n.BuildingId " +
                $"\r\n\t\t, r.id as RoleId " +
                $"\r\n\t\t, n.ShortContent  " +
                $"\r\n\t\t, n.LongContent " +

                $"\r\n  FROM Notifies n " +
                $"\r\n  join Buildings b on n.BuildingId = b.id " +
                $"\r\n  left join Roles r on r.Id = n.RoleId" +
                $"\r\n  left join Residents re on re.Id = n.InsertedById  " +
                $"\r\n  left join Staff sa on sa.Id = n.InsertedById" +
                $"\r\n  left join Residents reUpdate on reUpdate .Id = n.UserAccpectId  " +
                $"\r\n  left join Staff saUpdate on saUpdate.Id = n.UserAccpectId";

            #region Where
            query += $"\r\n\tWhere n.id = @id";
            param.Add("@id", "" + notifyId + "");

            #endregion

            #region Order by
            query += $"\r\n\r\n  Order by n.InsertedAt desc ";

            #endregion

            return await _dapperCommon.QueryAsync<NotifyDetailResponseDto>(query, param);
        }
    }
}
