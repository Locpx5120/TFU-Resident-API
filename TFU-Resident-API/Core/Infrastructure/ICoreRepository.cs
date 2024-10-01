using System.Linq.Expressions;

namespace Core.Infrastructure
{
    public interface ICoreRepository<T>
    {
        void Add(T entity);
        void Add(IEnumerable<T> entities);

        void Update(T entity);
        void Update(IEnumerable<T> entities);

        void Delete(T entity, bool isHardDelete = false);

        void Delete(IEnumerable<T> entities, bool isHardDelete = false);

        void Delete(Expression<Func<T, bool>> where, bool isHardDelete = false);

        IQueryable<T> GetQuery();
        IQueryable<T> GetQuery(Expression<Func<T, bool>> where);
    }
}
