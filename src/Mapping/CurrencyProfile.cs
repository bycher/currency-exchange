using AutoMapper;
using CurrencyExchange.Api.Models.Entities;
using CurrencyExchange.Api.Models.Requests;
using CurrencyExchange.Api.Models.Responses;

namespace CurrencyExchange.Api.Mapping;

/// <summary>
/// Mapping profile for currency's related models.
/// </summary>
public class CurrencyProfile : Profile {
    public CurrencyProfile() {
        CreateMap<Currency, CurrencyResponse>()
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.FullName));

        CreateMap<CurrencyResponse, Currency>()
            .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.Name));

        CreateMap<CreateCurrencyRequest, Currency>()
            .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.Name));
    }
}