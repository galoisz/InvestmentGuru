using Persistence.Data.Repositories.Interfaces;

namespace Persistence.Data.UnitOfWork;

public class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _dbContext;

    public IStockRepository Stocks { get; }
    public IPriceRepository Prices { get; }
    public IProtfolioPeriodRepository ProtfolioPeriodRepository { get; }
    public IProtfolioRepository ProtfolioRepository { get; }
    public IProtfolioStockRepository ProtfolioStockRepository { get; }


    public UnitOfWork(ApplicationDbContext dbContext, IStockRepository stockRepository, IPriceRepository priceRepository)
    {
        _dbContext = dbContext;
        Stocks = stockRepository;
        Prices = priceRepository;
    }

    public async Task CompleteAsync()
    {
        await _dbContext.SaveChangesAsync();
    }

    public void Dispose()
    {
        _dbContext.Dispose();
    }
}
