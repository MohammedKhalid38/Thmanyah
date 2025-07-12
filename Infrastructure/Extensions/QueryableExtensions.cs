using System.Linq.Expressions;
using System.Reflection;

namespace Infrastructure.Extensions;
public static class QueryableExtensions
{
    public static IQueryable<T> SearchAllColumns<T>(this IQueryable<T> source, string? keyword)
    {
        if (string.IsNullOrWhiteSpace(keyword))
            return source;

        var parameter = Expression.Parameter(typeof(T), "x");
        Expression? predicate = null;

        foreach (var property in typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance)
                                          .Where(p => p.PropertyType == typeof(string)))
        {
            var propertyAccess = Expression.Property(parameter, property);
            var nullCheck = Expression.NotEqual(propertyAccess, Expression.Constant(null, typeof(string)));
            var containsMethod = typeof(string).GetMethod(nameof(string.Contains), new[] { typeof(string) })!;
            var keywordExpression = Expression.Constant(keyword, typeof(string));
            var containsExpression = Expression.Call(propertyAccess, containsMethod, keywordExpression);
            var combined = Expression.AndAlso(nullCheck, containsExpression);

            predicate = predicate == null ? combined : Expression.OrElse(predicate, combined);
        }

        if (predicate == null)
            return source;

        var lambda = Expression.Lambda<Func<T, bool>>(predicate, parameter);
        return source.Where(lambda);
    }



}