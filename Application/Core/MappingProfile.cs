using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Core;

using Application.Dtos;
using AutoMapper;
using Persistence.Entities;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<ProtfolioDto, Protfolio>().ReverseMap();
        CreateMap<ProtfolioStockDto, ProtfolioStock>().ReverseMap();
        CreateMap<ProtfolioPeriodDto, ProtfolioPeriod>().ReverseMap();
    }
}
