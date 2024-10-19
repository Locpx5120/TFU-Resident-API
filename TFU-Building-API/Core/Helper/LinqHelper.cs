using System.ComponentModel;
using System.Linq.Expressions;

namespace TFU_Building_API.Core.Helper
{
    public static class LinqHelper
    {
        public static IQueryable<T> OrderByCustom<T>(this IQueryable<T> query, string sortColumn, string direction)
        {
            var methodName = string.Format("OrderBy{0}", direction == "asc" ? "" : "descending");

            var parameter = Expression.Parameter(query.ElementType, "p");

            MemberExpression memberAccess = null;
            foreach (var property in sortColumn.Split('.'))
            {
                memberAccess = MemberExpression.Property(memberAccess ?? (parameter as Expression), property);
            }

            var orderByLambda = Expression.Lambda(memberAccess, parameter);

            var result = Expression.Call(
                typeof(Queryable),
                methodName,
                new[] { query.ElementType, memberAccess.Type },
                query.Expression,
                Expression.Quote(orderByLambda));

            return query.Provider.CreateQuery<T>(result);
        }
        public static IQueryable<T> FilterCustom<T>(this IQueryable<T> queryable, string fieldName, string keyWords)
        {
            try
            {
                var entityType = typeof(T);
                var parameter = Expression.Parameter(entityType, "a");
                var propertyExpression = Expression.Property(parameter, fieldName);
                Expression body = Expression.Constant(false);
                MethodCallExpression innerExpression;
                if (propertyExpression.Type.IsValueType)
                {
                    object value = TypeDescriptor.GetConverter(propertyExpression.Type).ConvertFromString(keyWords);
                    var equal = Expression.Equal(propertyExpression, Expression.Constant(value, propertyExpression.Type));
                    body = Expression.OrElse(body, equal);
                }
                else
                {
                    var containsMethod = typeof(string).GetMethod("Contains", new[] { typeof(string) });
                    innerExpression = Expression.Call(propertyExpression, containsMethod, Expression.Constant(keyWords));
                    body = Expression.OrElse(body, innerExpression);
                }

                var lambda = Expression.Lambda<Func<T, bool>>(body, new[] { parameter });
                return queryable.Where(lambda);
            }
            catch (Exception e)
            {
                return queryable;
            }

        }
    }
}