using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StockLibrary.CQRS.Commands;
using StockLibrary.CQRS.Queries;
using StockLibrary.Data;
using StockLibrary.Data.Repositories;
using StockLibrary.Data.UnitOfWork;

namespace StocksLibrary.Helpers;

public static class ApplicationServiceExtensions
{
    public static void AddServices(this IServiceCollection services, IConfiguration config) {
        services.AddDbContext<ApplicationDbContext>(options => options.UseNpgsql(config.GetConnectionString("StocksConnection")));
        services.AddScoped<IStockRepository, StockRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddMediatR(config =>
        {
            config.RegisterServicesFromAssembly(typeof(CreateStockCommandHandler).Assembly);
            config.RegisterServicesFromAssembly(typeof(GetStockQueryHandler).Assembly);
        });
    }
}
