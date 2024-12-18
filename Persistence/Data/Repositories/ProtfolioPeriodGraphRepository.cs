using Microsoft.EntityFrameworkCore;
using Persistence.Data.Repositories.Interfaces;
using Persistence.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Data.Repositories;

public class ProtfolioPeriodGraphRepository : IProtfolioPeriodGraphRepository
{
    private readonly ApplicationDbContext _context;

    public ProtfolioPeriodGraphRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<ProtfolioPeriodGraph>> GetAllAsync()
    {
        return await _context.ProtfolioPeriodGraphs
            .Include(p => p.ProtfolioPeriod) // Include navigation property if needed
            .ToListAsync();
    }

    public async Task<ProtfolioPeriodGraph?> GetByIdAsync(Guid id)
    {
        return await _context.ProtfolioPeriodGraphs
            .Include(p => p.ProtfolioPeriod) // Include navigation property if needed
            .FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task AddAsync(ProtfolioPeriodGraph protfolioPeriodGraph)
    {
        await _context.ProtfolioPeriodGraphs.AddAsync(protfolioPeriodGraph);
    }

    public async Task UpdateAsync(ProtfolioPeriodGraph protfolioPeriodGraph)
    {
        _context.ProtfolioPeriodGraphs.Update(protfolioPeriodGraph);
    }

    public async Task DeleteAsync(Guid id)
    {
        var entity = await _context.ProtfolioPeriodGraphs.FindAsync(id);
        if (entity != null)
        {
            _context.ProtfolioPeriodGraphs.Remove(entity);
        }
    }
}
