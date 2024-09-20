using WebApi.Data.Entities;

namespace WebApi.Data.Repositories;

public interface IDataRepository
{
    Task<IEnumerable<Stock>> GetAllAsync();
    Task<Stock> GetByIdAsync(int id);
    Task<Stock> AddAsync(Stock Stock);
    Task<Stock> UpdateAsync(Stock Stock);
    Task<Stock> DeleteAsync(int id);
}
