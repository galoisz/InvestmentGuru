using StockLibrary.Data.Repositories;
using StocksLibrary.Data.Repositories;

namespace StockLibrary.Data.UnitOfWork;
public interface IUnitOfWork : IDisposable
{
    IStockRepository Stocks { get; }
    IPriceRepository Prices { get; }
    Task CompleteAsync();
}
