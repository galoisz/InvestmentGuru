using Microsoft.AspNetCore.Mvc;
using WebApi.Services.Finance;

namespace WebApi.Controllers.Finance;

[Route("api/[controller]")]
[ApiController]
public class FinanceController : ControllerBase
{
    private readonly IFinanceService _financeService;

    public FinanceController(IFinanceService financeService)
    {
        _financeService = financeService;
    }

    [HttpGet("prices")]
    public async Task<IActionResult> GetDailyPrices(string symbol, string fromDate, string toDate)
    {
        try
        {
            var prices = await _financeService.GetDailyPrices(symbol, fromDate, toDate);
            return Ok(prices);
        }
        catch (HttpRequestException ex)
        {
            return StatusCode(500, $"Error retrieving data: {ex.Message}");
        }
    }
}
