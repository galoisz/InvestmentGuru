using Application.Dtos;
using AutoMapper;
using MediatR;
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

    public Handler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
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
            await _unitOfWork.ProtfolioPeriodRepository.AddAsync(newPeriod);
        }

        // Commit the transaction
        await _unitOfWork.CompleteAsync();

        return Unit.Value;
    }
}
