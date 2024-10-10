namespace StockLibrary.Data.Entities;

public class Stock
{
    public Guid Id { get; set; }
    public string Symbol { get; set; }
    public string Prices { get; set; } // JSON representation of List<PriceEntry>
}