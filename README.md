Navz.NetExtensions.Linq




A collection of useful LINQ-related C# extensions designed to simplify and enhance data querying operations.

🚀 Features

FilterBySearchKeyword: Perform keyword-based filtering on multiple properties.

More extensions to come...

📦 Installation

You can install this package via NuGet:

 dotnet add package Navz.NetExtensions.Linq

Or via the NuGet Package Manager:

Install-Package Navz.NetExtensions.Linq

🛠 Usage

using Navz.NetExtensions.Linq.Extensions;

var data = new List<TestEntity>
{
    new() { Name = "Apple", Description = "Red fruit" },
    new() { Name = "Banana", Description = "Yellow fruit" },
    new() { Name = "Green Apple", Description = "Sour fruit" }
}.AsQueryable();

var result = data.FilterBySearchKeyword("green apple", x => x.Name).ToList();

// Output: Matches only "Green Apple"

📂 Repository Structure

📦 Navz.NetExtensions
├── 📂 src
│   ├── 📂 Navz.NetExtensions.Linq
│   │   ├── QueryableExtensions.cs
│   │   ├── Navz.NetExtensions.Linq.csproj
├── 📂 tests
│   ├── 📂 Navz.NetExtensions.Linq.Tests
│   │   ├── QueryableExtensionsTests.cs
│   │   ├── Navz.NetExtensions.Linq.Tests.csproj
├── .gitignore
├── Navz.NetExtensions.Linq.nuspec
├── README.md
├── Navz.NetExtensions.sln

✅ Running Tests

Run the unit tests with:

dotnet test

📜 License

This project is licensed under the MIT License. See LICENSE for details.

🤝 Contributing

Contributions are welcome! Feel free to submit a PR or open an issue.

⭐ Support

If you find this project useful, please ⭐ it on GitHub!