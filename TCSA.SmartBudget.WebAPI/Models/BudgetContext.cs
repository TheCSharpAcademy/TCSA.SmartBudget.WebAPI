using Microsoft.EntityFrameworkCore;

namespace TCSA.SmartBudget.WebAPI.Models;

public class BudgetContext(DbContextOptions<BudgetContext> options) : DbContext(options)
{
    public virtual DbSet<BudgetRecord> BudgetRecords { get; set; }

    public virtual DbSet<Category> Categories { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(20);
        });
        modelBuilder.Entity<BudgetRecord>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Description).IsRequired().HasMaxLength(30);
            entity.Property(e => e.Amount).IsRequired().HasColumnType("decimal(18,2)");
            entity.Property(e => e.Date).IsRequired();
            entity.HasOne(e => e.Category)
                  .WithMany()
                  .HasForeignKey(e => e.CategoryId)
                  .OnDelete(DeleteBehavior.Cascade);
        });
    }
}
