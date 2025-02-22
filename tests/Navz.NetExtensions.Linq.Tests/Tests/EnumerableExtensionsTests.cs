using Navz.NetExtensions.Linq.Extensions;

namespace Navz.NetExtensions.Linq.Tests.Tests;
public class EnumerableExtensionsTests
{
    [Fact]
    public void Batch_SplitsCorrectly()
    {
        var numbers = Enumerable.Range(1, 10);
        var batches = numbers.Batch(3).ToList();

        Assert.Equal(4, batches.Count);
        Assert.Equal(new[] { 1, 2, 3 }, batches[0].ToArray());
        Assert.Equal(new[] { 4, 5, 6 }, batches[1].ToArray());
        Assert.Equal(new[] { 7, 8, 9 }, batches[2].ToArray());
        Assert.Equal(new[] { 10 }, batches[3].ToArray());
    }

    [Fact]
    public void Batch_ThrowsOnNullSource()
    {
        IEnumerable<int> numbers = null;
        Assert.Throws<ArgumentNullException>(() => numbers.Batch(3).ToList());
    }

    [Fact]
    public void Batch_ThrowsOnInvalidBatchSize()
    {
        var numbers = Enumerable.Range(1, 10);
        Assert.Throws<ArgumentOutOfRangeException>(() => numbers.Batch(0).ToList());
    }
}
