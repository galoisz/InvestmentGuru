using Application.Dtos;
using Persistence.Data.UnitOfWork;
using MediatR;

namespace Application.Stocks;

public class List
{

    public class Query : IRequest<StockDto>
    {
        public string Symbol { get; set; }
    }


    public class Handler : IRequestHandler<Query, StockDto>
    {
        private readonly IUnitOfWork _unitOfWork;

        public Handler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<StockDto> Handle(Query request, CancellationToken cancellationToken)
        {
            var stock = await _unitOfWork.StocksRepository.GetStockBySymbolAsync(request.Symbol);

            if (stock == null) return null;

            var prices = new List<PriceDto>();// JsonConvert.DeserializeObject<List<PriceEntry>>(stock.Prices);
            return new StockDto { Symbol = stock.Symbol, Prices = prices };
        }
    }



}
