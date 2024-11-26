using Persistence.Data.Repositories.Interfaces;

namespace Persistence.Data.UnitOfWork;
public interface IUnitOfWork : IDisposable
{
    IStockRepository Stocks { get; }
    IPriceRepository Prices { get; }
    IProtfolioPeriodRepository ProtfolioPeriodRepository { get; }
    IProtfolioRepository ProtfolioRepository { get; }
    IProtfolioStockRepository ProtfolioStockRepository { get; }
    Task CompleteAsync();
}
