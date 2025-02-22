namespace Navz.NetExtensions.Linq.Extensions;

/// <summary>
/// Provides extension methods for IEnumerable.
/// </summary>
public static class EnumerableExtensions
{
    /// <summary>
    /// Batches elements of the input sequence into smaller sequences of the specified size.
    /// </summary>
    /// <typeparam name="T">The entity type.</typeparam>
    /// <param name="source">The queryable data source.</param>
    /// <param name="size">The size of each batch.</param>
    /// <returns>An IEnumerable of IEnumerable of T representing batches of elements.</returns>
    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    public static IEnumerable<IEnumerable<T>> Batch<T>(
        this IEnumerable<T> source, 
        int size)
    {
        if (source == null) throw new ArgumentNullException(nameof(source));
        if (size <= 0) throw new ArgumentOutOfRangeException(nameof(size), "Batch size must be greater than zero.");

        using (var enumerator = source.GetEnumerator())
        {
            while (enumerator.MoveNext())
            {
                var batch = new List<T> { enumerator.Current };

                while (batch.Count < size && enumerator.MoveNext())
                {
                    batch.Add(enumerator.Current);
                }

                yield return batch;
            }
        }
    }
}
