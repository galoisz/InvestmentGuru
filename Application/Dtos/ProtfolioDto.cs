namespace Application.Dtos;

public class ProtfolioDto
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public string Name { get; set; }
    public decimal Budget { get; set; }
    public List<ProtfolioStockDto> Stocks { get; set; }
    public List<ProtfolioPeriodDto> Periods { get; set; }
}