namespace Application.Models;

public class PriceEntry
{
    public decimal Open { get; set; }
    public decimal High { get; set; }
    public decimal Low { get; set; }
    public decimal Close { get; set; }
    public string Date { get; set; }
    public long Volume { get; set; }
}