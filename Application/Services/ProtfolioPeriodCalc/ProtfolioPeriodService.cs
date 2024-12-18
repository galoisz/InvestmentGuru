using Application.Dtos;
using AutoMapper;
using MediatR;
using Newtonsoft.Json;
using Persistence.Data.UnitOfWork;
using Persistence.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.ProtfolioPeriodCalc;

public class ProtfolioPeriodCalcService : IProtfolioPeriodCalcService
{
    private readonly IUnitOfWork _unitOfWork;
    public ProtfolioPeriodCalcService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<List<PeriodicalValueDto>> Calculate(ProtfolioPeriod period, List<ProtfolioStockDto> stocks, decimal budget)
    {
        //// Prepare tasks for calculating revenues in parallel
        //var tasks = stocks.Select(stock => CalculatePeriodicalValue(period, stock, budget * stock.Ratio)).ToList();

        //// Await all tasks and flatten the results into a single list
        //var revenueResults = await Task.WhenAll(tasks);
        //var revenues = revenueResults.SelectMany(revenue => revenue).ToList();


        var revenues = new List<PeriodicalValueDto>();
        foreach (var stock in stocks)
        {
            var revenue = await CalculatePeriodicalValue(period, stock, budget * stock.Ratio);
            revenues.AddRange(revenue);
        }

        // Get distinct revenue dates and calculate total values for each date
        var revenueDates = revenues.Select(x => x.Date).Distinct().OrderBy(x => x).ToList();

        var result = revenueDates.Select(currdate => new PeriodicalValueDto
        {
            Date = currdate,
            Value = revenues.Where(x => x.Date == currdate).Sum(x => x.Value)
        }).ToList();

        return result;
    }


    private async Task<List<PeriodicalValueDto>> CalculatePeriodicalValue(ProtfolioPeriod period, ProtfolioStockDto stock, decimal budget)
    {
        Price price = await _unitOfWork.Prices.GetByStockIdAsync(stock.StockId);
        var priceList = JsonConvert.DeserializeObject<List<PriceDto>>(price.Value);



        var filteredPrices = priceList
            .Where(p => DateTime.Parse(p.Date) >= period.FromDate && DateTime.Parse(p.Date) <= period.ToDate)
            .OrderBy(p => DateTime.Parse(p.Date))
            .ToList();

        if (!filteredPrices.Any())
        {
            throw new ArgumentException("No prices available in the specified date range.");
        }

        // Get the opening price on the starting date
        var startPrice = filteredPrices.FirstOrDefault(p => DateTime.Parse(p.Date) >= period.FromDate)?.Open;

        if (startPrice == null)
        {
            throw new ArgumentException("No opening price available for the start date.");
        }

        // Calculate the number of stocks purchased and the remainder
        var stocksPurchased = Math.Floor(budget / startPrice.Value);
        var remainder = budget - (stocksPurchased * startPrice.Value);

        // Calculate the value for each day
        var values = filteredPrices.Select(p => new PeriodicalValueDto
        {
            Date = p.Date,
            Value = (stocksPurchased * p.Close) + remainder
        }).ToList();

        return values;
    }

}
