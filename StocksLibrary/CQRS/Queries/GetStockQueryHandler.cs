using MediatR;
using Newtonsoft.Json;
using StockLibrary.Data.UnitOfWork;
using StockLibrary.Models;

namespace StockLibrary.CQRS.Queries;

public class GetStockQueryHandler : IRequestHandler<GetStockQuery, StockEntry>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetStockQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<StockEntry> Handle(GetStockQuery request, CancellationToken cancellationToken)
    {
        var stock = await _unitOfWork.Stocks.GetStockBySymbolAsync(request.Symbol);

        if (stock == null) return null;

        var prices = new List<PriceEntry>();// JsonConvert.DeserializeObject<List<PriceEntry>>(stock.Prices);
        return new StockEntry { Symbol = stock.Symbol, Prices = prices };
    }
}
