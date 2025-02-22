using System.Linq.Expressions;

namespace Navz.NetExtensions.Linq.Extensions;

/// <summary>
/// Provides extension methods for IQueryable.
/// </summary>
public static class QueryableExtensions
{
    /// <summary>
    /// Filters an IQueryable based on a search keyword across multiple properties.
    /// </summary>
    /// <typeparam name="T">The entity type.</typeparam>
    /// <param name="query">The queryable data source.</param>
    /// <param name="searchKeyword">The search keyword.</param>
    /// <param name="propertySelectors">The properties to search within.</param>
    /// <returns>Filtered IQueryable.</returns>
    /// <exception cref="ArgumentException"></exception>
    public static IQueryable<T> FilterBySearchKeyword<T>(
        this IQueryable<T> query,
        string? searchKeyword,
        params Expression<Func<T, string?>>[] propertySelectors)
    {
        if (string.IsNullOrWhiteSpace(searchKeyword) || propertySelectors.Length == 0)
        {
            return query;
        }

        var searchWords = searchKeyword
            .Trim()
            .ToLower()
            .Split(' ', StringSplitOptions.RemoveEmptyEntries);

        if (searchWords.Length == 0)
        {
            return query;
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
            return query;
        }

        var lambda = Expression.Lambda<Func<T, bool>>(combinedPropertiesExpression, parameter);
        return query.Where(lambda);
    }


    /// <summary>
    /// Batches elements of the queryable sequence into smaller sequences of the specified size.
    /// </summary>
    /// <typeparam name="T">The entity type.</typeparam>
    /// <param name="source">The queryable data source.</param>
    /// <param name="size">The size of each batch.</param>
    /// <returns>An IEnumerable of List of T representing batches of elements, ensuring immediate execution.</returns>
    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    public static IEnumerable<List<T>> Batch<T>(this IQueryable<T> source, int size)
    {
        if (source == null) throw new ArgumentNullException(nameof(source));
        if (size <= 0) throw new ArgumentOutOfRangeException(nameof(size), "Batch size must be greater than zero.");

        int count = source.Count();
        for (int i = 0; i < count; i += size)
        {
            yield return source.Skip(i).Take(size).ToList(); // Materialize each batch immediately
        }
    }
}
