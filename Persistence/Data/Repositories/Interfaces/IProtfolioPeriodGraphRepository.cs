using Persistence.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Data.Repositories.Interfaces;

public interface IProtfolioPeriodGraphRepository
{
    Task<IEnumerable<ProtfolioPeriodGraph>> GetAllAsync();
    Task<ProtfolioPeriodGraph?> GetByIdAsync(Guid id);
    Task AddAsync(ProtfolioPeriodGraph protfolioPeriodGraph);
    Task UpdateAsync(ProtfolioPeriodGraph protfolioPeriodGraph);
    Task DeleteAsync(Guid id);
}

