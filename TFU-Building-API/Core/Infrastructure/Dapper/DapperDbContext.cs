using Microsoft.Data.SqlClient;
using System.Data;
using TFU_Building_API.Core.Struct;

namespace TFU_Building_API.Core.Infrastructure.Dapper
{
    public class DapperDbContext
    {
        private readonly string _connectionString;

        public DapperDbContext(IConfiguration conf)
        {
            _connectionString = conf[AppSetting.ConnectionStrings.DbConnection];
        }

        public IDbConnection CreateConnection() => new SqlConnection(_connectionString);
    }
}
