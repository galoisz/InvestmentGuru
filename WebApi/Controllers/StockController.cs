using MediatR;
using Microsoft.AspNetCore.Mvc;
using StockLibrary.CQRS.Queries;
using StockLibrary.Models;
using StocksLibrary.Stocks;

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
        var command = new CreateStockCommand
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
        var query = new GetStockQuery { Symbol = symbol };
        var stock = await _mediator.Send(query);
        if (stock == null) return NotFound();
        return Ok(stock);
    }
}
