using TCSA.SmartBudget.Models.Responses;
using TCSA.SmartBudget.WebAPI.Models;

namespace TCSA.SmartBudget.WebAPI.Services;

public interface IBudgetService
{
    Task<ServiceResponse<Category>> AddCategory(Category category); 
}

public class BudgetService(BudgetContext _context) : IBudgetService
{
    public Task<ServiceResponse<Category>> AddCategory(Category category)
    {
        try
        {
            _context.Categories.Add(category);
            _context.SaveChanges();
            return Task.FromResult(new ServiceResponse<Category>
            {
                Data = category,
                IsSuccessful = true,
                Message = "Category added successfully."
            });

        }
        catch (Exception ex)
        {
            return Task.FromResult(new ServiceResponse<Category>
            {
                Data = null,
                IsSuccessful = false,
                Message = $"Error adding category: {ex.Message}"
            });
        }
    }
}
