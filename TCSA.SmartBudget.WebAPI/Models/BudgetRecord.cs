namespace TCSA.SmartBudget.WebAPI.Models;

public class BudgetRecord
{
    public int Id { get; set; }
    public string Description { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public DateTime Date { get; set; }
    public required int CategoryId { get; set; }
    public Category Category { get; set; } = null!;
}
