# Navz.NetExtensions

A collection of useful C# extensions designed to simplify and reuse operations.

![Build Status](https://img.shields.io/github/actions/workflow/status/rmnavz/Navz.NetExtensions/.github%2Fworkflows%2Fci.yml)
![Release Status](https://img.shields.io/github/actions/workflow/status/rmnavz/Navz.NetExtensions/.github%2Fworkflows%2Frelease.yml?label=release)

**Navz.NetExtensions.Linq**: 
[![NuGet](https://img.shields.io/nuget/v/Navz.NetExtensions.Linq.svg)](https://www.nuget.org/packages/Navz.NetExtensions.Linq)
[![NuGet Downloads](https://img.shields.io/nuget/dt/Navz.NetExtensions.Linq.svg)](https://www.nuget.org/packages/Navz.NetExtensions.Linq)

## 🚀 Features

This repository contains multiple NuGet packages, each focusing on specific extension categories:

- **Navz.NetExtensions.Linq**: Extensions for LINQ operations.
- More extensions to come...

## 📦 Installation

You can install individual packages via NuGet. For example, to install the LINQ extensions:

```sh
dotnet add package Navz.NetExtensions.Linq
```

Or via the NuGet Package Manager:

```sh
Install-Package Navz.NetExtensions.Linq
```

## 🛠 Usage

```csharp
using Navz.NetExtensions.Linq.Extensions;

var data = new List<TestEntity>
{
    new() { Name = "Apple", Description = "Red fruit" },
    new() { Name = "Banana", Description = "Yellow fruit" },
    new() { Name = "Green Apple", Description = "Sour fruit" }
}.AsQueryable();

var result = data.FilterBySearchKeyword("green apple", x => x.Name).ToList();

// Output: Matches only "Green Apple"
```

```csharp
var numbers = Enumerable.Range(1, 10);
var batches = numbers.Batch(3).ToList();

// Output: List of enumerables, each containing up to 3 elements
```

```csharp
var data = new List<TestEntity>
    {
        new() { Name = "Apple", Description = "Red fruit" },
        new() { Name = "Banana", Description = "Yellow fruit" },
        new() { Name = "Grape", Description = "Purple fruit" }
    }.AsQueryable();

var result = data.WhereIf(x => x.Name == "Apple", true).ToList();

// Output: List of entity if condition is met, otherwise unmodified source.
```

```csharp
var data = new List<TestEntity>
    {
        new() { Name = "Banana", Description = "Yellow fruit" },
        new() { Name = "Apple", Description = "Red fruit" },
        new() { Name = "Grape", Description = "Purple fruit" }
    }.AsQueryable();

var result = data.OrderByProperty("Name").ToList();

// Output: List of entity ordered ascending or descending.
```

## 📂 Repository Structure

```
📦 Navz.NetExtensions
├── 📂 src
│   ├── 📂 Navz.NetExtensions.Linq
│   │   ├── QueryableExtensions.cs
│   │   ├── Navz.NetExtensions.Linq.csproj
│   ├── 📂 Navz.NetExtensions.String
│   │   ├── StringExtensions.cs
│   │   ├── Navz.NetExtensions.String.csproj
│   ├── 📂 Navz.NetExtensions.Int
│   │   ├── IntExtensions.cs
│   │   ├── Navz.NetExtensions.Int.csproj
├── 📂 tests
│   ├── 📂 Navz.NetExtensions.Linq.Tests
│   │   ├── QueryableExtensionsTests.cs
│   │   ├── Navz.NetExtensions.Linq.Tests.csproj
│   ├── 📂 Navz.NetExtensions.String.Tests
│   │   ├── StringExtensionsTests.cs
│   │   ├── Navz.NetExtensions.String.Tests.csproj
│   ├── 📂 Navz.NetExtensions.Int.Tests
│   │   ├── IntExtensionsTests.cs
│   │   ├── Navz.NetExtensions.Int.Tests.csproj
├── .gitignore
├── Navz.NetExtensions.Linq.nuspec
├── README.md
├── Navz.NetExtensions.sln
```

## ✅ Running Tests

Run the unit tests with:

```sh
dotnet test
```

## 📜 License

This project is licensed under the MIT License. See [LICENSE](LICENSE) for details.

## 🤝 Contributing

Contributions are welcome! Feel free to submit a PR or open an issue.

## ⭐ Support

If you find this project useful, please ⭐ it on [GitHub](https://github.com/rmnavz/Navz.NetExtensions)!