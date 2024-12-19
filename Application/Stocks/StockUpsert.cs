using Application.Dtos;
using Persistence.Data.UnitOfWork;
using Persistence.Entities;
using MediatR;
using Newtonsoft.Json;

namespace Application.Stocks;

public class StockUpsertCommand : IRequest<Unit>
{
    public string Symbol { get; set; }
    public List<PriceDto> Prices { get; set; }
}

public class StockUpsertHandler : IRequestHandler<StockUpsertCommand, Unit>
{
    private readonly IUnitOfWork _unitOfWork;

    public StockUpsertHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Unit> Handle(StockUpsertCommand request, CancellationToken cancellationToken)
    {
        // Serialize the Prices list into a JSON string
        var serializedPrices = JsonConvert.SerializeObject(request.Prices);

        DateTime? minPriceDate = request.Prices.Any() && DateTime.TryParse(request.Prices.First().Date, out var parsedMinDate)
            ? DateTime.SpecifyKind(parsedMinDate, DateTimeKind.Utc)
            : null;

        DateTime? maxPriceDate = request.Prices.Any() && DateTime.TryParse(request.Prices.Last().Date, out var parsedMaxDate)
            ? DateTime.SpecifyKind(parsedMaxDate, DateTimeKind.Utc)
            : null;

        // Check if the stock exists
        var existingStock = await _unitOfWork.StocksRepository.GetStockBySymbolAsync(request.Symbol);

        if (existingStock == null)
        {
            // If stock doesn't exist, create a new one
            var newStock = new Stock
            {
                Id = Guid.NewGuid(),
                Symbol = request.Symbol,
                Name = $"{request.Symbol} Stock",
                MinPriceDate = minPriceDate,
                MaxPriceDate = maxPriceDate
            };
            await _unitOfWork.StocksRepository.AddStockAsync(newStock);

            // Create a price entry for the new stock
            var newPrice = new Price
            {
                Id = Guid.NewGuid(),
                StockId = newStock.Id,
                Value = serializedPrices
            };
            await _unitOfWork.PricesRepository.AddAsync(newPrice);
        }
        else
        {
            // If stock exists, update it
            existingStock.Name = $"{request.Symbol} Stock (Updated)";
            existingStock.MinPriceDate = minPriceDate;
            existingStock.MaxPriceDate = maxPriceDate;
            await _unitOfWork.StocksRepository.UpdateStockAsync(existingStock);

            // Get existing prices for the stock
            var existingPrice = await _unitOfWork.PricesRepository.GetByStockIdAsync(existingStock.Id);

            if (existingPrice != null)
            {
                existingPrice.Value = serializedPrices;
                await _unitOfWork.PricesRepository.UpdateAsync(existingPrice);
            }
            else
            {
                // Add a new price entry if no prices exist
                var newPrice = new Price
                {
                    Id = Guid.NewGuid(),
                    StockId = existingStock.Id,
                    Value = serializedPrices
                };
                await _unitOfWork.PricesRepository.AddAsync(newPrice);
            }
        }

        // Commit all changes
        await _unitOfWork.CompleteAsync();

        return Unit.Value;
    }
}
