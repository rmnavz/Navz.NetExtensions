using System.Dynamic;
using System.Linq.Expressions;

namespace Navz.NetExtensions.Linq.Extensions;

/// <summary>
/// Provides extension methods for IQueryable, enhancing query operations.
/// </summary>
public static class QueryableExtensions
{
    /// <summary>
    /// Filters an IQueryable based on a search keyword across multiple properties.
    /// </summary>
    /// <typeparam name="T">The entity type.</typeparam>
    /// <param name="source">The queryable data source.</param>
    /// <param name="searchKeyword">The search keyword.</param>
    /// <param name="propertySelectors">The properties to search within.</param>
    /// <returns>Filtered IQueryable.</returns>
    /// <exception cref="ArgumentException">Thrown when property selectors contain unsupported expressions.</exception>
    public static IQueryable<T> FilterBySearchKeyword<T>(
        this IQueryable<T> source,
        string? searchKeyword,
        params Expression<Func<T, string?>>[] propertySelectors)
    {
        if (string.IsNullOrWhiteSpace(searchKeyword) || propertySelectors.Length == 0)
        {
            return source;
        }

        var searchWords = searchKeyword
            .Trim()
            .ToLower()
            .Split(' ', StringSplitOptions.RemoveEmptyEntries);

        if (searchWords.Length == 0)
        {
            return source;
        }

        var parameter = Expression.Parameter(typeof(T), "x");
        Expression? combinedPropertiesExpression = null;

        foreach (var selector in propertySelectors)
        {
            if (selector.Body is not MemberExpression propertyAccess)
            {
                throw new ArgumentException("Only direct member access is supported.", nameof(propertySelectors));
            }

            var property = Expression.MakeMemberAccess(parameter, propertyAccess.Member);
            var isNotNull = Expression.NotEqual(property, Expression.Constant(null, typeof(string)));

            Expression? allWordsExpression = null;

            foreach (var word in searchWords)
            {
                var containsCall = Expression.Call(
                    property,
                    nameof(string.Contains),
                    Type.EmptyTypes,
                    Expression.Constant(word, typeof(string)),
                    Expression.Constant(StringComparison.OrdinalIgnoreCase)
                );

                var containsCheck = Expression.AndAlso(isNotNull, containsCall);

                allWordsExpression = allWordsExpression == null
                    ? containsCheck
                    : Expression.AndAlso(allWordsExpression, containsCheck);
            }

            if (allWordsExpression != null)
            {
                combinedPropertiesExpression = combinedPropertiesExpression == null
                    ? allWordsExpression
                    : Expression.OrElse(combinedPropertiesExpression, allWordsExpression);
            }
        }

        if (combinedPropertiesExpression == null)
        {
            return source;
        }

        var lambda = Expression.Lambda<Func<T, bool>>(combinedPropertiesExpression, parameter);
        return source.Where(lambda);
    }

    /// <summary>
    /// Applies a conditional filter to an IQueryable.
    /// </summary>
    /// <typeparam name="T">The entity type.</typeparam>
    /// <param name="source">The queryable data source.</param>
    /// <param name="predicate">Filter condition.</param>
    /// <param name="applyCondition">Determines whether to apply the filter.</param>
    /// <returns>Filtered IQueryable if condition is met, otherwise unmodified source.</returns>
    public static IQueryable<T> WhereIf<T>(
        this IQueryable<T> source,
        Expression<Func<T, bool>> predicate,
        bool applyCondition)
    {
        return applyCondition ? source.Where(predicate) : source;
    }

    /// <summary>
    /// Batches elements of the queryable sequence into smaller collections of a specified size.
    /// </summary>
    /// <typeparam name="T">The entity type.</typeparam>
    /// <param name="source">The queryable data source.</param>
    /// <param name="batchSize">The number of elements per batch.</param>
    /// <returns>A sequence of batches, each as a List of elements.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when batchSize is not greater than zero.</exception>
    public static IEnumerable<List<T>> Batch<T>(
        this IQueryable<T> source, 
        int batchSize)
    {
        if (batchSize <= 0)
            throw new ArgumentOutOfRangeException(nameof(batchSize), "Batch size must be greater than zero.");

        int count = source.Count();
        for (int i = 0; i < count; i += batchSize)
        {
            yield return source.Skip(i).Take(batchSize).ToList();
        }
    }

    /// <summary>
    /// Orders an IQueryable by a specified property name dynamically.
    /// </summary>
    /// <typeparam name="T">The entity type.</typeparam>
    /// <param name="source">The queryable data source.</param>
    /// <param name="propertyName">The property to order by.</param>
    /// <param name="descending">Whether to sort in descending order.</param>
    /// <returns>Ordered IQueryable.</returns>
    public static IQueryable<T> OrderByProperty<T>(
        this IQueryable<T> source, 
        string propertyName, 
        bool descending = false)
    {
        var parameter = Expression.Parameter(typeof(T), "x");
        var property = Expression.Property(parameter, propertyName);
        var lambda = Expression.Lambda(property, parameter);

        string methodName = descending ? "OrderByDescending" : "OrderBy";
        var result = typeof(Queryable).GetMethods()
            .First(m => m.Name == methodName && m.GetParameters().Length == 2)
            .MakeGenericMethod(typeof(T), property.Type)
            .Invoke(null, new object[] { source, lambda });

        return (IQueryable<T>)result!;
    }
}
