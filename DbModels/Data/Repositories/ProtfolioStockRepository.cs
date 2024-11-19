using DbModels.Data.Repositories.Interfaces;
using DbModels.Entities;
using Microsoft.EntityFrameworkCore;

namespace DbModels.Data.Repositories;

public class ProtfolioStockRepository : IProtfolioStockRepository
{
    private readonly ApplicationDbContext _context;

    public ProtfolioStockRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<ProtfolioStock>> GetAllAsync()
    {
        return await _context.Set<ProtfolioStock>()
            .Include(ps => ps.Stock)
            .ToListAsync();
    }

    public async Task<ProtfolioStock?> GetByIdAsync(Guid id)
    {
        return await _context.Set<ProtfolioStock>()
            .Include(ps => ps.Stock)
            .FirstOrDefaultAsync(ps => ps.Id == id);
    }

    public async Task AddAsync(ProtfolioStock protfolioStock)
    {
        await _context.Set<ProtfolioStock>().AddAsync(protfolioStock);
    }

    public async Task UpdateAsync(ProtfolioStock protfolioStock)
    {
        _context.Set<ProtfolioStock>().Update(protfolioStock);
    }

    public async Task DeleteAsync(Guid id)
    {
        var stock = await GetByIdAsync(id);
        if (stock != null)
        {
            _context.Set<ProtfolioStock>().Remove(stock);
        }
    }
}
