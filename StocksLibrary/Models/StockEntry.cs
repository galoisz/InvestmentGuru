namespace StockLibrary.Models;

public class StockEntry
{
    public string Symbol { get; set; }
    public List<PriceEntry> Prices { get; set; }
}