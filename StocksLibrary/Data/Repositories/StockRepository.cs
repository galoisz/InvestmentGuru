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

    public async Task<Stock> GetStockBySymbolAsync(string symbol)
    {
        return await _context.Stocks.FirstOrDefaultAsync(s => s.Symbol == symbol);
    }

    public async Task AddStockAsync(Stock stock)
    {
        await _context.Stocks.AddAsync(stock);
    }
}