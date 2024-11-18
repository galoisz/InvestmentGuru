using DbModels.Data.Repositories;

namespace DbModels.Data.UnitOfWork;
public interface IUnitOfWork : IDisposable
{
    IStockRepository Stocks { get; }
    IPriceRepository Prices { get; }
    Task CompleteAsync();
}
