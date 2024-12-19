using Microsoft.EntityFrameworkCore;
using Persistence.Data.Repositories;
using Persistence.Data.Repositories.Interfaces;
using System;

namespace Persistence.Data.UnitOfWork;

public class UnitOfWork : IUnitOfWork
{
    private readonly IDbContextFactory<ApplicationDbContext> _dbContextFactory;
    private readonly ApplicationDbContext _dbContext;

    public IStockRepository StocksRepository { get; }
    public IPriceRepository PricesRepository { get; }
    public IProtfolioPeriodRepository ProtfolioPeriodRepository { get; }
    public IProtfolioRepository ProtfolioRepository { get; }
    public IProtfolioStockRepository ProtfolioStockRepository { get; }
    public IProtfolioPeriodGraphRepository ProtfolioPeriodGraphRepository { get; }



    public UnitOfWork(IDbContextFactory<ApplicationDbContext> dbContextFactory)
    {
        _dbContextFactory = dbContextFactory;
        _dbContext = _dbContextFactory.CreateDbContext();
        StocksRepository = new StockRepository(_dbContext);
        PricesRepository = new PriceRepository(_dbContext);
        ProtfolioPeriodRepository = new ProtfolioPeriodRepository(_dbContext);
        ProtfolioRepository = new ProtfolioRepository(_dbContext);
        ProtfolioStockRepository = new ProtfolioStockRepository(_dbContext);
        ProtfolioPeriodGraphRepository = new ProtfolioPeriodGraphRepository(_dbContext);
    }

    public IPriceRepository GetneratePricesRepository()
    {
        return new PriceRepository(_dbContextFactory.CreateDbContext());
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
