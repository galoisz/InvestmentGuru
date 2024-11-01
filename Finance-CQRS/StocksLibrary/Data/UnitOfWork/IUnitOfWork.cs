using StockLibrary.Data.Repositories;

namespace StockLibrary.Data.UnitOfWork;

public interface IUnitOfWork : IDisposable
{
    IStockRepository Stocks { get; }
    Task<int> CompleteAsync();
}