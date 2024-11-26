using Persistence.Entities;

namespace Persistence.Data.Repositories.Interfaces;

public interface IPriceRepository
{
    Task AddAsync(Price price);
    Task<Price?> GetByIdAsync(Guid id);
    Task<IEnumerable<Price>> GetByStockIdAsync(Guid stockId);
    Task UpdateAsync(Price price);
    Task DeleteAsync(Guid id);
}

