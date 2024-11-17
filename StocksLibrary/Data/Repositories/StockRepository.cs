using DbModels;
using DbModels.Entities;
using Microsoft.EntityFrameworkCore;

namespace StockLibrary.Data.Repositories;
public class StockRepository : IStockRepository
{
    private readonly ApplicationDbContext _context;

    public StockRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Stock?> GetStockBySymbolAsync(string symbol)
    {
        return await _context.Stocks.FirstOrDefaultAsync(s => s.Symbol == symbol);
    }

    public async Task<Stock?> GetByIdAsync(Guid id)
    {
        return await _context.Stocks.FindAsync(id);
    }

    public async Task<IEnumerable<Stock>> GetAllAsync()
    {
        return await _context.Stocks.ToListAsync();
    }

    public async Task AddStockAsync(Stock stock)
    {
        await _context.Stocks.AddAsync(stock);
    }

    public async Task UpdateStockAsync(Stock stock)
    {
        var existingStock = await _context.Stocks.FindAsync(stock.Id);
        if (existingStock != null)
        {
            existingStock.Symbol = stock.Symbol;
            existingStock.Name = stock.Name;
            existingStock.MinPriceDate = stock.MinPriceDate;
            existingStock.MaxPriceDate = stock.MaxPriceDate;

            _context.Stocks.Update(existingStock);
        }
    }

    public async Task DeleteStockAsync(Guid id)
    {
        var stock = await _context.Stocks.FindAsync(id);
        if (stock != null)
        {
            _context.Stocks.Remove(stock);
        }
    }
}
