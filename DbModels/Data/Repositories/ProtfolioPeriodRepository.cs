using DbModels.Data.Repositories.Interfaces;
using DbModels.Entities;
using Microsoft.EntityFrameworkCore;

namespace DbModels.Data.Repositories;

public class ProtfolioPeriodRepository : IProtfolioPeriodRepository
{
    private readonly ApplicationDbContext _context;

    public ProtfolioPeriodRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<ProtfolioPeriod>> GetAllAsync()
    {
        return await _context.Set<ProtfolioPeriod>().ToListAsync();
    }

    public async Task<ProtfolioPeriod?> GetByIdAsync(Guid id)
    {
        return await _context.Set<ProtfolioPeriod>()
            .FirstOrDefaultAsync(pp => pp.Id == id);
    }

    public async Task AddAsync(ProtfolioPeriod protfolioPeriod)
    {
        await _context.Set<ProtfolioPeriod>().AddAsync(protfolioPeriod);
    }

    public async Task UpdateAsync(ProtfolioPeriod protfolioPeriod)
    {
        _context.Set<ProtfolioPeriod>().Update(protfolioPeriod);
    }

    public async Task DeleteAsync(Guid id)
    {
        var period = await GetByIdAsync(id);
        if (period != null)
        {
            _context.Set<ProtfolioPeriod>().Remove(period);
        }
    }
}
