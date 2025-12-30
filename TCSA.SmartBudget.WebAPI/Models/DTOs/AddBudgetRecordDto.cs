namespace TCSA.SmartBudget.WebAPI.Models.DTOs;

public class AddBudgetRecordDto
{
    public string Description { get; set; } 
    public decimal Amount { get; set; }
    public DateTime Date { get; set; }
    public int CategoryId { get; set;
    }
}
