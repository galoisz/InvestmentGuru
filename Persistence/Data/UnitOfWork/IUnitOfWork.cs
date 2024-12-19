using Persistence.Data.Repositories.Interfaces;

namespace Persistence.Data.UnitOfWork;
public interface IUnitOfWork : IDisposable
{
    IStockRepository StocksRepository { get; }
    IPriceRepository PricesRepository { get; }
    IPriceRepository GetneratePricesRepository();
    IProtfolioPeriodRepository ProtfolioPeriodRepository { get; }
    IProtfolioRepository ProtfolioRepository { get; }
    IProtfolioStockRepository ProtfolioStockRepository { get; }
    IProtfolioPeriodGraphRepository ProtfolioPeriodGraphRepository { get; }

    Task CompleteAsync();
}
