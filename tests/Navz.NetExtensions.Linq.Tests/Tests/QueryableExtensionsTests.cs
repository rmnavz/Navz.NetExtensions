using Navz.NetExtensions.Linq.Extensions;

namespace Navz.NetExtensions.Linq.Tests.Tests;
public class QueryableExtensionsTests
{
    private class TestEntity
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
    }

    [Fact]
    public void FilterBySearchKeyword_FiltersBySingleProperty()
    {
        var data = new List<TestEntity>
            {
                new() { Name = "Apple", Description = "Red fruit" },
                new() { Name = "Banana", Description = "Yellow fruit" },
                new() { Name = "Grape", Description = "Purple fruit" }
            }.AsQueryable();

        var result = data.FilterBySearchKeyword("apple", x => x.Name).ToList();

        Assert.Single(result);
        Assert.Equal("Apple", result[0].Name);
    }

    [Fact]
    public void FilterBySearchKeyword_MultipleWords_FindsMatches()
    {
        var data = new List<TestEntity>
            {
                new() { Name = "Apple", Description = "Red fruit" },
                new() { Name = "Banana", Description = "Yellow fruit" },
                new() { Name = "Green Apple", Description = "Sour fruit" }
            }.AsQueryable();

        var result = data.FilterBySearchKeyword("green apple", x => x.Name).ToList();

        Assert.Single(result);
        Assert.Equal("Green Apple", result[0].Name);
    }

    [Fact]
    public void FilterBySearchKeyword_FiltersAcrossMultipleProperties()
    {
        var data = new List<TestEntity>
            {
                new() { Name = "Apple", Description = "Red fruit" },
                new() { Name = "Banana", Description = "Yellow fruit" },
                new() { Name = "Grape", Description = "Purple fruit" }
            }.AsQueryable();

        var result = data.FilterBySearchKeyword("red", x => x.Name, x => x.Description).ToList();

        Assert.Single(result);
        Assert.Equal("Apple", result[0].Name);
    }

    [Fact]
    public void FilterBySearchKeyword_NoMatch_ReturnsEmpty()
    {
        var data = new List<TestEntity>
            {
                new() { Name = "Apple", Description = "Red fruit" },
                new() { Name = "Banana", Description = "Yellow fruit" },
                new() { Name = "Grape", Description = "Purple fruit" }
            }.AsQueryable();

        var result = data.FilterBySearchKeyword("Orange", x => x.Name).ToList();

        Assert.Empty(result);
    }

    [Fact]
    public void Batch_SplitsCorrectly()
    {
        var numbers = Enumerable.Range(1, 10).AsQueryable();
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
        IQueryable<int> numbers = null;
        Assert.Throws<ArgumentNullException>(() => numbers.Batch(3).ToList());
    }

    [Fact]
    public void Batch_ThrowsOnInvalidBatchSize()
    {
        var numbers = Enumerable.Range(1, 10).AsQueryable();
        Assert.Throws<ArgumentOutOfRangeException>(() => numbers.Batch(0).ToList());
    }
}
