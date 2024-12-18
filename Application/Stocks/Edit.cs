using Application.Dtos;
using Persistence.Data.UnitOfWork;
using Persistence.Entities;
using MediatR;
using Newtonsoft.Json;

namespace Application.Stocks;

public class EditStockCommand : IRequest<Unit>
{
    public string Symbol { get; set; }
    public List<PriceDto> Prices { get; set; }
}


public class EditStockCommandHandler : IRequestHandler<EditStockCommand, Unit>
{
    private readonly IUnitOfWork _unitOfWork;

    public EditStockCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Unit> Handle(EditStockCommand request, CancellationToken cancellationToken)
    {
        // Check if the stock already exists
        var existingStock = await _unitOfWork.Stocks.GetStockBySymbolAsync(request.Symbol);

        if (existingStock == null)
        {
            throw new InvalidOperationException($"Stock with symbol {request.Symbol} does not exist.");
        }

        // Serialize the Prices list into a JSON string
        var serializedPrices = JsonConvert.SerializeObject(request.Prices);

        DateTime? minPriceDate = request.Prices.Any() && DateTime.TryParse(request.Prices.First().Date, out var parsedMinDate)
            ? DateTime.SpecifyKind(parsedMinDate, DateTimeKind.Utc)
            : null;

        DateTime? maxPriceDate = request.Prices.Any() && DateTime.TryParse(request.Prices.Last().Date, out var parsedMaxDate)
            ? DateTime.SpecifyKind(parsedMaxDate, DateTimeKind.Utc)
            : null;

        // Update existing stock details
        existingStock.Name = $"{request.Symbol} Stock (Updated)";
        existingStock.MinPriceDate = minPriceDate;
        existingStock.MaxPriceDate = maxPriceDate;
        await _unitOfWork.Stocks.UpdateStockAsync(existingStock);

        // Get existing prices for the stock
        var existingPrice = await _unitOfWork.Prices.GetByStockIdAsync(existingStock.Id);

        if (existingPrice != null)
        {
            // Update the first existing price with the serialized Prices
            existingPrice.Value = serializedPrices;
            await _unitOfWork.Prices.UpdateAsync(existingPrice);
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
            await _unitOfWork.Prices.AddAsync(newPrice);
        }

        // Commit all changes
        await _unitOfWork.CompleteAsync();

        return Unit.Value;
    }
}