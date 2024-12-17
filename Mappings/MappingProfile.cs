using AutoMapper;
using CurrencyExchange.Dto;
using CurrencyExchange.Models;

namespace CurrencyExchange.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Currency, CurrencyDto>();
        CreateMap<CurrencyDto, Currency>();
        CreateMap<CreateCurrencyDto, Currency>();
    }
}