using Core.Entity;
using System.Linq.Expressions;

namespace TFU_Building_API.Core.Infrastructure
{
    public interface IMasterDataRepository<T> : IRepository<T> where T : IMasterDataEntityBase
    {
        IEnumerable<T> GetAllWithInactive();
        IEnumerable<T> GetManyWithInactive(Expression<Func<T, bool>> where);
        IQueryable<T> GetQuery(bool includeInactive = false);
        new IQueryable<T> GetQuery(Expression<Func<T, bool>> where);
        IQueryable<T> GetQueryWithInactive(Expression<Func<T, bool>> where);
        IEnumerable<T> GetMany(Expression<Func<T, bool>> where);
    }
}