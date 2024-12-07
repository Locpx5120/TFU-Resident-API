using Microsoft.Data.SqlClient;
using System.Data;

namespace TFU_Building_API.Core.Infrastructure.Dapper
{
    public class DapperDbContext
    {
        private readonly string _connectionString;

        public DapperDbContext(IConfiguration conf)
        {
            //  _connectionString = conf[AppSetting.ConnectionStrings.DbConnection];
            _connectionString = "Data Source= 202.92.7.204,1437;Initial Catalog=QLToaNhaDb2_1;Persist Security Info=True;User ID=QLToaNhaDb2_1;Password=qe853B5f%;TrustServerCertificate=True";
        }

        public IDbConnection CreateConnection() => new SqlConnection(_connectionString);
    }
}
