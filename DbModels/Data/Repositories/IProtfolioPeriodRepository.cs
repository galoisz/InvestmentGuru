using DbModels.Entities;

namespace DbModels.Data.Repositories;

public interface IProtfolioPeriodRepository
{
    Task<IEnumerable<ProtfolioPeriod>> GetAllAsync();
    Task<ProtfolioPeriod?> GetByIdAsync(Guid id);
    Task AddAsync(ProtfolioPeriod protfolioPeriod);
    Task UpdateAsync(ProtfolioPeriod protfolioPeriod);
    Task DeleteAsync(Guid id);
}
