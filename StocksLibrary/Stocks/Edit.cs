﻿using DbModels.Data.UnitOfWork;
using DbModels.Entities;
using MediatR;
using Newtonsoft.Json;
using StockLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StocksLibrary.Stocks;

public class EditStockCommand : IRequest<Unit>
{
    public string Symbol { get; set; }
    public List<PriceEntry> Prices { get; set; }
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

        // Commit all changes
        await _unitOfWork.CompleteAsync();

        return Unit.Value;
    }
}