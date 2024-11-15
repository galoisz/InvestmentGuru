using DbModels.Entities;
using MediatR;
using Newtonsoft.Json;
using StockLibrary.Data.UnitOfWork;

namespace StockLibrary.CQRS.Commands;



public class CreateStockCommandHandler : IRequestHandler<CreateStockCommand, Unit>
{
    private readonly IUnitOfWork _unitOfWork;

    public CreateStockCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Unit> Handle(CreateStockCommand request, CancellationToken cancellationToken)
    {
        var stock = new Stock
        {
            Id = Guid.NewGuid(),
            Symbol = request.Symbol,
            //Prices = JsonConvert.SerializeObject(request.Prices)
        };

        await _unitOfWork.Stocks.AddStockAsync(stock);
        await _unitOfWork.CompleteAsync();
        return Unit.Value;
    }
}
