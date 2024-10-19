using Core.Entity;
using System.Linq.Expressions;

namespace TFU_Building_API.Core.Infrastructure
{
    public interface IRepository<T> : ICoreRepository<T> where T : IEntityBase
    {
        T GetById(Guid id);
        Task<T> GetByIdAsync(Guid id);
        IQueryable<T> GetQueryById(Guid id);
        Task<TResult> GetPropertyById<TResult>(Guid id, Expression<Func<T, TResult>> selector);
        IQueryable<T> SearchByQuery(List<string> fieldValues, List<string> fieldNames, bool? IsActive);
    }
}
