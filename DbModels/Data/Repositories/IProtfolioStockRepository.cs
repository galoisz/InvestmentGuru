using DbModels.Entities;

namespace DbModels.Data.Repositories;

public interface IProtfolioStockRepository
{
    Task<IEnumerable<ProtfolioStock>> GetAllAsync();
    Task<ProtfolioStock?> GetByIdAsync(Guid id);
    Task AddAsync(ProtfolioStock protfolioStock);
    Task UpdateAsync(ProtfolioStock protfolioStock);
    Task DeleteAsync(Guid id);
}
