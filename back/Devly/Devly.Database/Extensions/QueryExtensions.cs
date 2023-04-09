using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

namespace Devly.Database.Extensions;

public static class QueryExtensions
{
    public static IQueryable<T> IncludeSafe<T>(this IQueryable<T> query, Expression<Func<T, object>>? include)
        where T : class
    {
        return include == null
            ? query
            : query.Include(include);
    }
    
    public static IQueryable<T> IncludeMultiple<T>(this IQueryable<T> query, Expression<Func<T, object>>[]? includes)
        where T : class
    {
        return includes == null || includes.Length == 0
            ? query
            : includes.Aggregate(query, (c, e) => c.Include(e));
    }
}