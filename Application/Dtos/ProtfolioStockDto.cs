namespace Application.Dtos;


public class ProtfolioStockDto
{
    public Guid Id { get; set; }
    public Guid ProtfolioId { get; set; }
    public Guid StockId { get; set; }
    public decimal Ratio { get; set; }
}