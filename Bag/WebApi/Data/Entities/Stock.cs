namespace WebApi.Data.Entities;

public class Stock
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Prices { get; set; }
    public DateTime FromDate { get; set; }
    public DateTime ToDate { get; set; }
}