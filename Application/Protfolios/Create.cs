using Application.Dtos;
using Application.Services.ProtfolioPeriodCalc;
using AutoMapper;
using MediatR;
using Newtonsoft.Json;
using Persistence.Data.Repositories.Interfaces;
using Persistence.Data.UnitOfWork;
using Persistence.Entities;

namespace Application.Protfolios;

public class Command : IRequest<Unit>
{
    public ProtfolioDto Protfolio { get; set; }
}

public class Handler : IRequestHandler<Command, Unit>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IProtfolioPeriodCalcService _protfolioPeriodCalcService;

    public Handler(IUnitOfWork unitOfWork, IMapper mapper, IProtfolioPeriodCalcService protfolioPeriodCalcService)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _protfolioPeriodCalcService = protfolioPeriodCalcService;
    }
 

    public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
    {
        var protfolioDto = request.Protfolio;

        // Map ProtfolioDto to Protfolio
        var newProtfolio = _mapper.Map<Protfolio>(protfolioDto);
        newProtfolio.Id = Guid.NewGuid(); // Generate ID manually
        await _unitOfWork.ProtfolioRepository.AddAsync(newProtfolio);

        // Map and add ProtfolioStocks
        foreach (var stockDto in protfolioDto.Stocks)
        {
            var newStock = _mapper.Map<ProtfolioStock>(stockDto);
            newStock.ProtfolioId = newProtfolio.Id; // Assign foreign key
            await _unitOfWork.ProtfolioStockRepository.AddAsync(newStock);
        }

        // Map and add ProtfolioPeriods
        foreach (var periodDto in protfolioDto.Periods)
        {
            var newPeriod = _mapper.Map<ProtfolioPeriod>(periodDto);
            newPeriod.Id = Guid.NewGuid();
            newPeriod.ProtfolioId = newProtfolio.Id;

            var periodicalValues = await _protfolioPeriodCalcService.Calculate(newPeriod, protfolioDto.Stocks, protfolioDto.Budget);
            var newProtfolioPeriodGraph = new ProtfolioPeriodGraph { Id = Guid.NewGuid(), ProtfolioPeriodId = newPeriod.Id, Graphdata = JsonConvert.SerializeObject(periodicalValues) };
            await _unitOfWork.ProtfolioPeriodGraphRepository.AddAsync(newProtfolioPeriodGraph);

            newPeriod.FinalBudget = periodicalValues.LastOrDefault()?.Value;
            await _unitOfWork.ProtfolioPeriodRepository.AddAsync(newPeriod);

        }

        // Commit the transaction
        await _unitOfWork.CompleteAsync();

        return Unit.Value;
    }
}
