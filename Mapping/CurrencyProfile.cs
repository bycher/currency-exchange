using AutoMapper;
using CurrencyExchange.Models.Domain;
using CurrencyExchange.Models.Dto;

namespace CurrencyExchange.Mapping;

/// <summary>
/// Mapping profile for currency's related models.
/// </summary>
public class CurrencyProfile : Profile {
    public CurrencyProfile() {
        CreateMap<Currency, CurrencyDto>()
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.FullName));

        CreateMap<CurrencyDto, Currency>()
            .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.Name));

        CreateMap<CurrencyFormDto, Currency>()
            .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.Name));
    }
}