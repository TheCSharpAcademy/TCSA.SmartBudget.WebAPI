using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using TCSA.SmartBudget.WebAPI.Models;
using TCSA.SmartBudget.WebAPI.Services;

namespace TCSA.SmartBudget.WebAPI.Tests;

public class Tests
{
    protected DbContextOptions<BudgetContext> _options = default!;

    [OneTimeSetUp]
    public async Task OneTimeSetUp()
    {
        var dbName = $"TestDb_{Guid.NewGuid()}";
        var baseConnectionString = Environment.GetEnvironmentVariable("ConnectionStrings__DefaultConnection")
        ?? "Server=localhost,1433;User Id=sa;Password=Str0ngP@ssw0rd!123;TrustServerCertificate=True;Encrypt=False;";

        _options = new DbContextOptionsBuilder<BudgetContext>()
            .UseSqlServer(baseConnectionString)
            .Options;

        var connectionStringBuilder = new SqlConnectionStringBuilder(baseConnectionString)
        {
            InitialCatalog = dbName
        };

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
    public async Task AddValidCategory_Persists_Category()
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

    [Test]
    public async Task AddValidRecord_Persists_Record()
    {
        // Seed
        await using var seedContext = new BudgetContext(_options);
        seedContext.Categories.Add(new Category { Name = "Groceries" });
        await seedContext.SaveChangesAsync();

        // Arrange
        await using var arrangeContext = new BudgetContext(_options);
        var service = new BudgetService(arrangeContext);

        var record = new BudgetRecord
        {
            Description = "Weekly groceries",
            Amount = 150.75m,
            Date = DateTime.UtcNow,
            CategoryId = 1
        };

        // Act
        var response = await service.AddBudgetRecord(record);

        // Assert
        Assert.That(response.IsSuccessful, Is.True);

        await using var verifyContext = new BudgetContext(_options);
        var dbRecords = await verifyContext.BudgetRecords.ToListAsync();

        Assert.That(dbRecords.Count, Is.EqualTo(1));
        Assert.That(dbRecords[0].Description, Is.EqualTo("Weekly groceries"));
    }
}
