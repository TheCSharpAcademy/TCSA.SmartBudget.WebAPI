using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using TCSA.SmartBudget.WebAPI.Models;
using TCSA.SmartBudget.WebAPI.Options;
using TCSA.SmartBudget.WebAPI.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.Configure<Auth0Settings>(builder.Configuration.GetSection("Auth0"));
builder.Services.Configure<ConnectionStringsOptions>(builder.Configuration.GetSection("ConnectionStrings"));

builder.Services.AddControllers();

var auth0 = builder.Configuration.GetSection("Auth0").Get<Auth0Settings>()!;

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.Authority = $"https://{auth0.Domain}/";
        options.Audience = auth0.Audience;
    });

builder.Services.AddDbContext<BudgetContext>((sp, db) =>
{
    var cs = sp.GetRequiredService<IOptions<ConnectionStringsOptions>>().Value.DefaultConnection;
    db.UseSqlServer(cs);
});

builder.Services.AddScoped<IBudgetService, BudgetService>();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<BudgetContext>();
    dbContext.Database.EnsureDeleted();
    dbContext.Database.EnsureCreated();
}


// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
