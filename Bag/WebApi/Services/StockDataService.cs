using WebApi.Data.Entities;
using WebApi.Data.Repositories;

namespace WebApi.Services;

public class StockDataService : IStockDataService
{
    private readonly IDataRepository _repository;

    public StockDataService(IDataRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<Stock>> GetAllAsync()
    {
        return await _repository.GetAllAsync();
    }

    public async Task<Stock> GetByIdAsync(int id)
    {
        return await _repository.GetByIdAsync(id);
    }

    public async Task<Stock> AddAsync(Stock Stock)
    {
        return await _repository.AddAsync(Stock);
    }

    public async Task<Stock> UpdateAsync(int id, Stock Stock)
    {
        if (id != Stock.Id)
        {
            return null;
        }

        return await _repository.UpdateAsync(Stock);
    }

    public async Task<Stock> DeleteAsync(int id)
    {
        return await _repository.DeleteAsync(id);
    }
}
