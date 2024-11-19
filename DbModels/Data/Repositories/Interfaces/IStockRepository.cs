
using DbModels.Entities;

namespace DbModels.Data.Repositories.Interfaces;

public interface IStockRepository
{
    Task<Stock?> GetStockBySymbolAsync(string symbol);
    Task<Stock?> GetByIdAsync(Guid id);
    Task<IEnumerable<Stock>> GetAllAsync();
    Task AddStockAsync(Stock stock);
    Task UpdateStockAsync(Stock stock);
    Task DeleteStockAsync(Guid id);
}
