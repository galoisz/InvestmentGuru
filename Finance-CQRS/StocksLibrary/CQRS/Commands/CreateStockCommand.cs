using MediatR;
using StockLibrary.Models;

namespace StockLibrary.CQRS.Commands;

public class CreateStockCommand : IRequest<Unit>
{
    public string Symbol { get; set; }
    public List<PriceEntry> Prices { get; set; }
}
