using DbModels.Data.Repositories.Interfaces;
using DbModels.Entities;
using Microsoft.EntityFrameworkCore;

namespace DbModels.Data.Repositories;

public class PriceRepository : IPriceRepository
{
    private readonly ApplicationDbContext _dbContext;

    public PriceRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task AddAsync(Price price)
    {
        await _dbContext.Set<Price>().AddAsync(price);
    }

    public async Task<Price?> GetByIdAsync(Guid id)
    {
        return await _dbContext.Set<Price>().FindAsync(id);
    }

    public async Task<IEnumerable<Price>> GetByStockIdAsync(Guid stockId)
    {
        return await _dbContext.Set<Price>()
            .Where(p => p.StockId == stockId)
            .ToListAsync();
    }

    public async Task UpdateAsync(Price price)
    {
        var existingPrice = await _dbContext.Set<Price>().FindAsync(price.Id);
        if (existingPrice != null)
        {
            existingPrice.Value = price.Value;
            _dbContext.Set<Price>().Update(existingPrice);
        }
    }

    public async Task DeleteAsync(Guid id)
    {
        var price = await _dbContext.Set<Price>().FindAsync(id);
        if (price != null)
        {
            _dbContext.Set<Price>().Remove(price);
        }
    }
}
