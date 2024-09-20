using Microsoft.EntityFrameworkCore;
using WebApi.Data.Entities;

namespace WebApi.Data.Repositories;

public class DataRepository : IDataRepository
{
    private readonly DataContext _context;

    public DataRepository(DataContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Stock>> GetAllAsync()
    {
        return await _context.Stocks.ToListAsync();
    }

    public async Task<Stock> GetByIdAsync(int id)
    {
        return await _context.Stocks.FindAsync(id);
    }

    public async Task<Stock> AddAsync(Stock Stock)
    {
        _context.Stocks.Add(Stock);
        await _context.SaveChangesAsync();
        return Stock;
    }

    public async Task<Stock> UpdateAsync(Stock Stock)
    {
        _context.Entry(Stock).State = EntityState.Modified;
        await _context.SaveChangesAsync();
        return Stock;
    }

    public async Task<Stock> DeleteAsync(int id)
    {
        var Stock = await _context.Stocks.FindAsync(id);
        if (Stock != null)
        {
            _context.Stocks.Remove(Stock);
            await _context.SaveChangesAsync();
        }
        return Stock;
    }
}