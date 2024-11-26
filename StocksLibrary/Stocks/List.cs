using Application.Models;
using DbModels.Data.UnitOfWork;
using MediatR;

namespace Application.Stocks;

public class List
{

    public class Query : IRequest<StockEntry>
    {
        public string Symbol { get; set; }
    }


    public class Handler : IRequestHandler<Query, StockEntry>
    {
        private readonly IUnitOfWork _unitOfWork;

        public Handler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<StockEntry> Handle(Query request, CancellationToken cancellationToken)
        {
            var stock = await _unitOfWork.Stocks.GetStockBySymbolAsync(request.Symbol);

            if (stock == null) return null;

            var prices = new List<PriceEntry>();// JsonConvert.DeserializeObject<List<PriceEntry>>(stock.Prices);
            return new StockEntry { Symbol = stock.Symbol, Prices = prices };
        }
    }



}
