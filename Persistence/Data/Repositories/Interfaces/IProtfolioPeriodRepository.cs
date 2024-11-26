using Persistence.Entities;

namespace Persistence.Data.Repositories.Interfaces;

public interface IProtfolioPeriodRepository
{
    Task<IEnumerable<ProtfolioPeriod>> GetAllAsync();
    Task<ProtfolioPeriod?> GetByIdAsync(Guid id);
    Task AddAsync(ProtfolioPeriod protfolioPeriod);
    Task UpdateAsync(ProtfolioPeriod protfolioPeriod);
    Task DeleteAsync(Guid id);
}
