using Microsoft.EntityFrameworkCore;
using TCSA.SmartBudget.WebAPI.Models;
using TCSA.SmartBudget.WebAPI.Services;

namespace TCSA.SmartBudget.WebAPI.Tests;

public class Tests
{
    protected DbContextOptions<BudgetContext> _options = default!;
    private string _connectionString = default!;

    [OneTimeSetUp]
    public async Task OneTimeSetUp()
    {
        var dbName = $"TestDb_{Guid.NewGuid()}";
        _connectionString = $"Data Source=.;Initial Catalog={dbName};Integrated Security=True;TrustServerCertificate=True";
        _options = new DbContextOptionsBuilder<BudgetContext>()
            .UseSqlServer(_connectionString)
            .Options;

        await using var context = new BudgetContext(_options);
        await context.Database.EnsureCreatedAsync();
    }

    [OneTimeTearDown]
    public async Task OneTimeTearDown()
    {
        await using var context = new BudgetContext(_options);
        await context.Database.EnsureDeletedAsync();
    }

    [SetUp]
    public async Task SetUp()
    {
        await using var context = new BudgetContext(_options);

        if (context.Set<Category>() is not null)
            await context.Set<Category>().ExecuteDeleteAsync();

        if (context.Set<BudgetRecord>() is not null)
            await context.Set<BudgetRecord>().ExecuteDeleteAsync();
    }

    [Test]
    public async Task AddValidProduct_Persists_Product()
    {
        // Arrange
        await using var arrangeContext = new BudgetContext(_options);
        var service = new BudgetService(arrangeContext);

        var category = new Category
        {
            Name = "Groceries",
        };

        // Act
        var response = await service.AddCategory(category);

        // Assert
        Assert.That(response.IsSuccessful, Is.True);

        await using var verifyContext = new BudgetContext(_options);
        var dbCategories = await verifyContext.Categories.ToListAsync();

        Assert.That(dbCategories.Count, Is.EqualTo(1));
        Assert.That(dbCategories[0].Name, Is.EqualTo("Groceries"));
    }
}
