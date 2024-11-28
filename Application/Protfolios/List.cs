using Application.Dtos;
using AutoMapper;
using MediatR;
using Persistence.Data.Repositories.Interfaces;
using Persistence.Data.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Protfolios;

public class List
{

    public class Query : IRequest<ProtfolioDto>
    {
        public Guid Id { get; set; }
    }


    public class Handler : IRequestHandler<Query, ProtfolioDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IProtfolioRepository _protfolioRepository;
        private IMapper _mapper;

        public Handler(IUnitOfWork unitOfWork, IProtfolioRepository protfolioRepository, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _protfolioRepository = protfolioRepository;
            _mapper = mapper;
        }

        public async Task<ProtfolioDto> Handle(Query request, CancellationToken cancellationToken)
        {

            var protfolio = _unitOfWork.ProtfolioRepository.GetByIdAsync(request.Id);
            if (protfolio == null) return null;

            return _mapper.Map<ProtfolioDto>(protfolio);
        }
    }
}
