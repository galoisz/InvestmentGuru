using DbModels.Entities;
using Microsoft.EntityFrameworkCore;

namespace DbModels;

public class ApplicationDbContext : DbContext
{
    public DbSet<Stock> Stocks { get; set; }
    public DbSet<Price> Prices { get; set; }
    public DbSet<Protfolio> Protfolios { get; set; }
    public DbSet<ProtfolioPeriod> ProtfolioPeriods { get; set; }
    public DbSet<ProtfolioStock> ProtfolioStocks { get; set; }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    //protected override void OnModelCreating(ModelBuilder modelBuilder)
    //{
    //    modelBuilder.Entity<Stock>().ToTable("stocks");
    //    modelBuilder.Entity<Stock>().ToTable("stocks");
    //}
}