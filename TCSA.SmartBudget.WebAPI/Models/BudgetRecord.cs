namespace TCSA.SmartBudget.WebAPI.Models;

public class BudgetRecord
{
    public int Id { get; set; }
    public string Description { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public DateTime Date { get; set; }
    public int CategoryId { get; set; }
    public required Category Category { get; set; } = null!;
}
