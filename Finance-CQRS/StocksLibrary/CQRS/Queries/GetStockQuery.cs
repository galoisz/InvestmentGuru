using MediatR;
using StockLibrary.Models;

namespace StockLibrary.CQRS.Queries;

public class GetStockQuery : IRequest<StockEntry>
{
    public string Symbol { get; set; }
}