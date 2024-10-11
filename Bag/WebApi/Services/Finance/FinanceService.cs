using Microsoft.Extensions.Options;
using System.Text.Json;
using WebApi.Helpers.Finance;

namespace WebApi.Services.Finance;

public class FinanceService : IFinanceService
{
    private readonly HttpClient _httpClient;
    private readonly string _financeApiBaseUrl;

    public FinanceService(HttpClient httpClient, IOptions<FinanceApiOptions> options)
    {
        _httpClient = httpClient;
        _financeApiBaseUrl = options.Value.BaseUrl;
    }

    public async Task<List<DailyPrice>> GetDailyPrices(string symbol, string fromDate, string toDate)
    {
        var url = $"{_financeApiBaseUrl}?symbol={symbol}&startDate={fromDate}&endDate={toDate}";

        var response = await _httpClient.GetAsync(url);
        response.EnsureSuccessStatusCode();

        var content = await response.Content.ReadAsStringAsync();
        var prices = JsonSerializer.Deserialize<List<DailyPrice>>(content, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });

        return prices ?? new List<DailyPrice>();
    }
}