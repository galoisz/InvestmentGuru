using DbModels.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbModels.Data.Repositories;

public class ProtfolioRepository : IProtfolioRepository
{
    private readonly ApplicationDbContext _context;

    public ProtfolioRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Protfolio>> GetAllAsync()
    {
        return await _context.Set<Protfolio>()
            .Include(p => p.Stocks)
            .Include(p => p.Periods)
            .ToListAsync();
    }

    public async Task<Protfolio?> GetByIdAsync(Guid id)
    {
        return await _context.Set<Protfolio>()
            .Include(p => p.Stocks)
            .Include(p => p.Periods)
            .FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task AddAsync(Protfolio protfolio)
    {
        await _context.Set<Protfolio>().AddAsync(protfolio);
    }

    public async Task UpdateAsync(Protfolio protfolio)
    {
        _context.Set<Protfolio>().Update(protfolio);
    }

    public async Task DeleteAsync(Guid id)
    {
        var protfolio = await GetByIdAsync(id);
        if (protfolio != null)
        {
            _context.Set<Protfolio>().Remove(protfolio);
        }
    }
}
