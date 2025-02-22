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
}
