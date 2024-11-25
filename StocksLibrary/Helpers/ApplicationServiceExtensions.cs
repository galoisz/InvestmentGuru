using DbModels;
using DbModels.Data.Repositories;
using DbModels.Data.Repositories.Interfaces;
using DbModels.Data.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using StocksLibrary.Stocks;

namespace StocksLibrary.Helpers;

public static class ApplicationServiceExtensions
{
    public static void AddServices(this IServiceCollection services, IConfiguration config) {
        services.AddDbContext<ApplicationDbContext>(options => options.UseNpgsql(config.GetConnectionString("StocksConnection")).LogTo(Console.WriteLine, LogLevel.Information));
        services.AddScoped<IStockRepository, StockRepository>();
        services.AddScoped<IPriceRepository, PriceRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddMediatR(config =>
        {
            config.RegisterServicesFromAssembly(typeof(Handler).Assembly);
            config.RegisterServicesFromAssembly(typeof(EditStockCommandHandler).Assembly);
            config.RegisterServicesFromAssembly(typeof(StocksLibrary.Stocks.List.Handler).Assembly);
        });
    }
}
