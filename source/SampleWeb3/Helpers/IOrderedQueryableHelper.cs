using System.Linq;
using System.Linq.Expressions;

namespace SampleWeb3.Helpers
{
    public static class IOrderedQueryableHelper
    {
        /// <summary>
        /// Orders the by.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source">The source.</param>
        /// <param name="propertyName">Name of the property.</param>
        /// <returns></returns>
        public static IOrderedQueryable<T> OrderBy<T>(this IQueryable<T> source,
                                                      string propertyName)
        {
            return OrderingHelper(source, propertyName, false, false);
        }

        /// <summary>
        /// Orders the by descending.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source">The source.</param>
        /// <param name="propertyName">Name of the property.</param>
        /// <returns></returns>
        public static IOrderedQueryable<T> OrderByDescending<T>(this IQueryable<T> source,
                                                                string propertyName)
        {
            return OrderingHelper(source, propertyName, true, false);
        }


        /// <summary>
        /// Thens the by.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source">The source.</param>
        /// <param name="propertyName">Name of the property.</param>
        /// <returns></returns>
        public static IOrderedQueryable<T> ThenBy<T>(this IOrderedQueryable<T> source,
                                                     string propertyName)
        {
            return OrderingHelper(source, propertyName, false, true);
        }

        /// <summary>
        /// Thens the by descending.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source">The source.</param>
        /// <param name="propertyName">Name of the property.</param>
        /// <returns></returns>
        public static IOrderedQueryable<T> ThenByDescending<T>(this IOrderedQueryable<T> source,
                                                               string propertyName)
        {
            return OrderingHelper(source, propertyName, true, true);
        }

        /// <summary>
        /// Orderings the helper.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source">The source.</param>
        /// <param name="propertyName">Name of the property.</param>
        /// <param name="descending">if set to <c>true</c> [descending].</param>
        /// <param name="anotherLevel">if set to <c>true</c> [another level].</param>
        /// <returns></returns>
        private static IOrderedQueryable<T> OrderingHelper<T>(IQueryable<T> source,
                                                              string propertyName,
                                                              bool descending,
                                                              bool anotherLevel)
        {
            ParameterExpression param = Expression.Parameter(typeof(T), string.Empty);
            MemberExpression property = Expression.PropertyOrField(param, propertyName);
            LambdaExpression sort = Expression.Lambda(property, param);

            MethodCallExpression call = Expression.Call(
                typeof(Queryable),
                string.Concat(
                    ( !anotherLevel
                        ? "OrderBy"
                        : "ThenBy" ),
                    ( descending
                        ? "Descending"
                        : string.Empty )),
                new[] { typeof(T), property.Type },
                source.Expression,
                Expression.Quote(sort));

            return (IOrderedQueryable<T>)source.Provider.CreateQuery<T>(call);
        }
    }
}