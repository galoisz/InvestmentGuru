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
    public IProtfolioPeriodGraphRepository ProtfolioPeriodGraphRepository { get; }


    
    public UnitOfWork(ApplicationDbContext dbContext, IStockRepository stocks, IPriceRepository prices, IProtfolioPeriodRepository protfolioPeriodRepository, IProtfolioRepository protfolioRepository, IProtfolioStockRepository protfolioStockRepository, IProtfolioPeriodGraphRepository protfolioPeriodGraphRepository)
    {
        _dbContext = dbContext;
        Stocks = stocks;
        Prices = prices;
        ProtfolioPeriodRepository = protfolioPeriodRepository;
        ProtfolioRepository = protfolioRepository;
        ProtfolioStockRepository = protfolioStockRepository;
        ProtfolioPeriodGraphRepository = protfolioPeriodGraphRepository;
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
