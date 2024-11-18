using DbModels;
using DbModels.Data.Repositories;
using DbModels.Data.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using StockLibrary.CQRS.Commands;
using StockLibrary.CQRS.Queries;

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
            config.RegisterServicesFromAssembly(typeof(CreateStockCommandHandler).Assembly);
            config.RegisterServicesFromAssembly(typeof(GetStockQueryHandler).Assembly);
        });
    }
}
