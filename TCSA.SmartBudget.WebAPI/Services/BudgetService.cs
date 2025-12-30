using TCSA.SmartBudget.Models.Responses;
using TCSA.SmartBudget.WebAPI.Models;

namespace TCSA.SmartBudget.WebAPI.Services;

public interface IBudgetService
{
    Task<ServiceResponse<Category>> AddCategory(Category category);
    Task<ServiceResponse<BudgetRecord>> AddBudgetRecord(BudgetRecord record);

}

public class BudgetService(BudgetContext _context) : IBudgetService
{
    public async Task<ServiceResponse<BudgetRecord>> AddBudgetRecord(BudgetRecord record)
    {
        try
        {
            await _context.BudgetRecords.AddAsync(record);
            await _context.SaveChangesAsync();

            return new ServiceResponse<BudgetRecord>
            {
                Data = record,
                IsSuccessful = true,
                Message = "Record added successfully."
            };

        }
        catch (Exception ex)
        {
            return new ServiceResponse<BudgetRecord>
            {
                Data = null,
                IsSuccessful = false,
                Message = $"Error adding record: {ex.Message}"
            };
        }
    }
    public async Task<ServiceResponse<Category>> AddCategory(Category category)
    {
        try
        {
            await _context.Categories.AddAsync(category);
            await _context.SaveChangesAsync();

            return new ServiceResponse<Category>
            {
                Data = category,
                IsSuccessful = true,
                Message = "Category added successfully."
            };

        }
        catch (Exception ex)
        {
            return new ServiceResponse<Category>
            {
                Data = null,
                IsSuccessful = false,
                Message = $"Error adding category: {ex.Message}"
            };
        }
    }
}
