using System;
using System.Net.Http;
using System.IO;
using System.Threading.Tasks;

class Program
{
    private static async Task Main(string[] args)
    {
        string symbol = "AAPL";
        string startDate = "2024-01-01";
        string endDate = "2024-01-10";
        string interval = "1d";

        string baseUrl = "https://query1.finance.yahoo.com/v7/finance/download/";

        // Convert dates to Unix timestamps (seconds since 1970-01-01)
        long startTimestamp = DateTimeOffset.Parse(startDate).ToUnixTimeSeconds();
        long endTimestamp = DateTimeOffset.Parse(endDate).ToUnixTimeSeconds();

        string url = $"{baseUrl}{symbol}?period1={startTimestamp}&period2={endTimestamp}&interval={interval}&events=history&includeAdjustedClose=true";

        url = "http://localhost:3000/finance/daily-prices?symbol=aapl&startDate=2020-01-01&endDate=2021-01-01";


        using (HttpClient client = new HttpClient())
        {
            try
            {
                Console.WriteLine($"Fetching stock data for {symbol}...");

                HttpResponseMessage response = await client.GetAsync(url);
                response.EnsureSuccessStatusCode();

                string csvData = await response.Content.ReadAsStringAsync();
                await Console.Out.WriteLineAsync(csvData);
                // Save CSV data to file
                //string filePath = $"{symbol}_StockData.csv";
                //await File.WriteAllTextAsync(filePath, csvData);

                //Console.WriteLine($"Data saved to {filePath}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }
    }
}
