namespace Application.Dtos;

public class ProtfolioPeriodDto
{
    public Guid Id { get; set; }
    public Guid ProtfolioId { get; set; }
    public DateTime FromDate { get; set; }
    public DateTime ToDate { get; set; }
}