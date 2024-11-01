using DataConsumer.Services.StockPrices.Models;
using Microsoft.Extensions.Configuration;
using RestSharp;

namespace DataConsumer.Services.StockPrices;


public class StockPriceService : IStockPriceService
{
    private readonly RestClient _client;
    private readonly string _baseUrl;

    public StockPriceService(IConfiguration configuration)
    {
        _baseUrl = configuration["StockApi:BaseUrl"];
    
        _client = new RestClient(_baseUrl);
    }

    public async Task<List<StockPrice>> GetStockPrices(string symbol, DateTime startDate, DateTime endDate)
    {
        var request = new RestRequest("daily-prices", Method.Get);
        request.AddParameter("symbol", symbol);
        request.AddParameter("startDate", startDate.ToString("yyyy-MM-dd"));
        request.AddParameter("endDate", endDate.ToString("yyyy-MM-dd"));

        var response = await _client.ExecuteAsync<List<StockPrice>>(request);
        if (response.IsSuccessful && response.Data != null)
        {
            return response.Data;
        }

        throw new Exception($"Error fetching stock prices: {response.ErrorMessage}");
    }


}
