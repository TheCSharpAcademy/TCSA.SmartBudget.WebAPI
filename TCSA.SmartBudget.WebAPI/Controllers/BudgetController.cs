using Microsoft.AspNetCore.Authorization;
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
    [Authorize]
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

    [Authorize]
    [HttpPost("add-transaction")]
    public async Task<IActionResult> AddTransaction([FromQuery] AddBudgetRecordDto category)
    {
        var mappedBudgetRecord = new BudgetRecord()
        {
            Description = category.Description.Trim(),
            Amount = category.Amount,
            Date = category.Date,
            CategoryId = category.CategoryId
        };

        var result = await _budgetService.AddBudgetRecord(mappedBudgetRecord);

        if (!result.IsSuccessful)
            return BadRequest(result.Message);

        return Ok(result.Data);
    }
}