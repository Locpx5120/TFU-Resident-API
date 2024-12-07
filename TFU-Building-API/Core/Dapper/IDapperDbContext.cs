using System.Data;

namespace TFU_Building_API.Core.Dapper
{
    public interface IDapperDbContext
    {
        public IDbConnection CreateConnection();
    }
}
