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

public class CreateStockCommand : IRequest<Unit>
{
    public string Symbol { get; set; }
    public List<PriceEntry> Prices { get; set; }
}

public class CreateStockCommandHandler : IRequestHandler<CreateStockCommand, Unit>
{
    private readonly IUnitOfWork _unitOfWork;

    public CreateStockCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Unit> Handle(CreateStockCommand request, CancellationToken cancellationToken)
    {
        // Serialize the Prices list into a JSON string
        var serializedPrices = JsonConvert.SerializeObject(request.Prices);

        DateTime? minPriceDate = request.Prices.Any() && DateTime.TryParse(request.Prices.First().Date, out var parsedMinDate)
            ? DateTime.SpecifyKind(parsedMinDate, DateTimeKind.Utc)
            : null;

        DateTime? maxPriceDate = request.Prices.Any() && DateTime.TryParse(request.Prices.Last().Date, out var parsedMaxDate)
            ? DateTime.SpecifyKind(parsedMaxDate, DateTimeKind.Utc)
            : null;

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

        // Commit all changes
        await _unitOfWork.CompleteAsync();

        return Unit.Value;
    }
}