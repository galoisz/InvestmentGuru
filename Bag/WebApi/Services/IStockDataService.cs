using WebApi.Data.Entities;

namespace WebApi.Services;

public interface IStockDataService
{
    Task<IEnumerable<Stock>> GetAllAsync();
    Task<Stock> GetByIdAsync(int id);
    Task<Stock> AddAsync(Stock Stock);
    Task<Stock> UpdateAsync(int id, Stock Stock);
    Task<Stock> DeleteAsync(int id);
}
