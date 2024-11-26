using Persistence.Entities;

namespace Persistence.Data.Repositories.Interfaces;

public interface IProtfolioRepository
{
    Task<IEnumerable<Protfolio>> GetAllAsync();
    Task<Protfolio?> GetByIdAsync(Guid id);
    Task AddAsync(Protfolio protfolio);
    Task UpdateAsync(Protfolio protfolio);
    Task DeleteAsync(Guid id);
}


