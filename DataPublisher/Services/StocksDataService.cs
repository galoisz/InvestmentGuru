using DataPublisher.Models;
using Microsoft.Extensions.Configuration;
using System.Text.Json;

namespace DataPublisher.Services;

public class StocksDataService : IStocksDataService
{
    private readonly string _filePath;

    public StocksDataService(IConfiguration configuration)
    {
        _filePath = configuration["StockFilePath"];
    }

    //public IEnumerable<StockEntry> GetStockData()
    //{
    //    var json = File.ReadAllText(_filePath);
    //    var stockData = JsonSerializer.Deserialize<IEnumerable<StockEntry>>(json);
    //    return stockData;
    //}


    public IEnumerable<StockEntry> GetStockData()
    {
        var json = File.ReadAllText(_filePath);
        var stockData = JsonSerializer.Deserialize<IEnumerable<StockEntry>>(json).ToList();

        for (var i = 0; i < 10000; i++) {

            StockEntry entry = new StockEntry { StockName = Guid.NewGuid().ToString(), MarketName="NASDAQ", FromDate = DateTime.Now.AddDays(new Random().Next(1000)), ToDate = DateTime.Now.AddDays(new Random().Next(1000)) };
            stockData.Add(entry);


        }
        
        
        return stockData;
    }
}
