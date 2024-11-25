using MediatR;
using Microsoft.AspNetCore.Mvc;
using StockLibrary.Models;
using StocksLibrary.Stocks;
using static StocksLibrary.Stocks.List;

namespace WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class StockController : ControllerBase
{
    private readonly IMediator _mediator;

    public StockController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<IActionResult> CreateStock([FromBody] StockEntry stockEntry)
    {
        var command = new Command
        {
            Symbol = stockEntry.Symbol,
            Prices = stockEntry.Prices
        };

        await _mediator.Send(command);
        return Ok();
    }

    [HttpGet("{symbol}")]
    public async Task<IActionResult> GetStock(string symbol)
    {
        var query = new Query { Symbol = symbol };
        var stock = await _mediator.Send(query);
        if (stock == null) return NotFound();
        return Ok(stock);
    }
}
