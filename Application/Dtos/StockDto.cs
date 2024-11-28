namespace Application.Dtos;

public class StockDto
{
    public string Symbol { get; set; }
    public List<PriceDto> Prices { get; set; }
}