namespace Application.Dtos;

public class PriceDto
{
    public decimal Open { get; set; }
    public decimal High { get; set; }
    public decimal Low { get; set; }
    public decimal Close { get; set; }
    public string Date { get; set; }
    public long Volume { get; set; }
}