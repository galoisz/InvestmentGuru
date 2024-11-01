using StockLibrary.Data.Repositories;

namespace StockLibrary.Data.UnitOfWork;

public class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _context;
    public IStockRepository Stocks { get; }

    public UnitOfWork(ApplicationDbContext context, IStockRepository stocks)
    {
        _context = context;
        Stocks = stocks;
    }

    public async Task<int> CompleteAsync()
    {
        return await _context.SaveChangesAsync();
    }

    public void Dispose()
    {
        _context.Dispose();
    }
}
