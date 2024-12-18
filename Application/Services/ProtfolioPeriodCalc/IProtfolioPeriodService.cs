using Application.Dtos;
using Persistence.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.ProtfolioPeriodCalc;

public interface IProtfolioPeriodCalcService
{
    Task<List<PeriodicalValueDto>> Calculate(ProtfolioPeriod period, List<ProtfolioStockDto> stocks, decimal budget);
}
