using Microsoft.EntityFrameworkCore;
using WebApi.Data.Entities;

namespace WebApi.Data;

public class DataContext : DbContext
{
    public DataContext(DbContextOptions<DataContext> options) : base(options)
    {
    }

    public DbSet<Stock> Stocks { get; set; }
}