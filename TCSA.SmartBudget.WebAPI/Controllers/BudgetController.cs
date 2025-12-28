using Microsoft.AspNetCore.Mvc;
using TCSA.SmartBudget.WebAPI.Models;
using TCSA.SmartBudget.WebAPI.Models.DTOs;
using TCSA.SmartBudget.WebAPI.Services;

namespace TCSA.SmartBudget.WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BudgetController(
    IBudgetService _budgetService,
    ILogger<BudgetController> _logger
    ) : ControllerBase
{
    [HttpPost("add-category")]
    public async Task<IActionResult> AddCategory([FromQuery] AddCategoryDto category)
    {
        var mappedCategory = new Category()
        {
            Name = category.Name.Trim()
        };

        var result = await _budgetService.AddCategory(mappedCategory);

        if (!result.IsSuccessful)
            return BadRequest(result.Message);

        return Ok(result.Data);
    }
}