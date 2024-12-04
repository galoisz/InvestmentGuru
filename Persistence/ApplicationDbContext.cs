using Persistence.Entities;
using Microsoft.EntityFrameworkCore;

namespace Persistence;

public class ApplicationDbContext : DbContext
{
    public DbSet<Stock> Stocks { get; set; }
    public DbSet<Price> Prices { get; set; }
    public DbSet<Protfolio> Protfolios { get; set; }
    public DbSet<ProtfolioPeriod> ProtfolioPeriods { get; set; }
    public DbSet<ProtfolioStock> ProtfolioStocks { get; set; }
    public DbSet<ProtfolioPeriodGraph> ProtfolioPeriodGraphs { get; set; }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Stock>()
            .HasIndex(s => s.Name)
            .IsUnique(); // Define the unique index
    }

}