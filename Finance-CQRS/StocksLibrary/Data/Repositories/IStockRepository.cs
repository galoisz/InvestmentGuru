using StockLibrary.Data.Entities;

namespace StockLibrary.Data.Repositories;

public interface IStockRepository
{
    Task<Stock> GetStockBySymbolAsync(string symbol);
    Task AddStockAsync(Stock stock);
}