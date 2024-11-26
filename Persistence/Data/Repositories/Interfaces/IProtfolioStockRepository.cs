using Persistence.Entities;

namespace Persistence.Data.Repositories.Interfaces;

public interface IProtfolioStockRepository
{
    Task<IEnumerable<ProtfolioStock>> GetAllAsync();
    Task<ProtfolioStock?> GetByIdAsync(Guid id);
    Task AddAsync(ProtfolioStock protfolioStock);
    Task UpdateAsync(ProtfolioStock protfolioStock);
    Task DeleteAsync(Guid id);
}
