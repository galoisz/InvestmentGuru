using DbModels.Entities;
using MediatR;
using Newtonsoft.Json;
using StockLibrary.Data.UnitOfWork;

namespace StockLibrary.CQRS.Commands;



public class CreateStockCommandHandler : IRequestHandler<CreateStockCommand, Unit>
{
    private readonly IUnitOfWork _unitOfWork;

    public CreateStockCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    public async Task<Unit> Handle(CreateStockCommand request, CancellationToken cancellationToken)
{
    // Check if the stock already exists
    var existingStock = await _unitOfWork.Stocks.GetStockBySymbolAsync(request.Symbol);

    // Serialize the Prices list into a JSON string
    var serializedPrices = JsonConvert.SerializeObject(request.Prices);

    // Initialize MinPriceDate and MaxPriceDate to null
    DateTime? minPriceDate = request.Prices.Any() && DateTime.TryParse(request.Prices.First().Date, out var parsedMinDate) ? parsedMinDate : null;
    DateTime? maxPriceDate = request.Prices.Any() && DateTime.TryParse(request.Prices.Last().Date, out var parsedMaxDate) ? parsedMaxDate : null;


    if (existingStock != null)
    {
        // Update existing stock details
        existingStock.Name = $"{request.Symbol} Stock (Updated)";
        existingStock.MinPriceDate = minPriceDate;
        existingStock.MaxPriceDate = maxPriceDate;
        await _unitOfWork.Stocks.UpdateStockAsync(existingStock);

        // Get existing prices for the stock
        var existingPrices = await _unitOfWork.Prices.GetByStockIdAsync(existingStock.Id);

        if (existingPrices.Any())
        {
            // Update the first existing price with the serialized Prices
            var existingPrice = existingPrices.First();
            existingPrice.Value = serializedPrices;
            await _unitOfWork.Prices.UpdateAsync(existingPrice);

            // Remove any extra prices to ensure there's only one price entry
            foreach (var extraPrice in existingPrices.Skip(1))
            {
                await _unitOfWork.Prices.DeleteAsync(extraPrice.Id);
            }
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
    }
    else
    {
        // Create a new stock
        var newStock = new Stock
        {
            Id = Guid.NewGuid(),
            Symbol = request.Symbol,
            Name = $"{request.Symbol} Stock",
            MinPriceDate = minPriceDate,
            MaxPriceDate = maxPriceDate
        };
        await _unitOfWork.Stocks.AddStockAsync(newStock);

        // Create a single price entry with the serialized Prices
        var newPrice = new Price
        {
            Id = Guid.NewGuid(),
            StockId = newStock.Id,
            Value = serializedPrices
        };
        await _unitOfWork.Prices.AddAsync(newPrice);
    }

    // Commit all changes
    await _unitOfWork.CompleteAsync();

    return Unit.Value;
}





}
