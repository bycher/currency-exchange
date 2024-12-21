using AutoMapper;
using CurrencyExchange.Api.Models.Entities;
using CurrencyExchange.Api.Models.Requests;
using CurrencyExchange.Api.Models.Responses;

namespace CurrencyExchange.Api.Mapping;

/// <summary>
/// Mapping profile for exchange rate's related models.
/// </summary>
public class ExchangeRateProfile : Profile {
    public ExchangeRateProfile() {
        // Ignore base and target currency properties because they are set in the service layer
        CreateMap<ExchangeRate, ExchangeRateResponse>()
            .ForMember(dest => dest.BaseCurrency, opt => opt.Ignore())
            .ForMember(dest => dest.TargetCurrency, opt => opt.Ignore());

        // Ignore base and target currency ids because they are set in the service layer
        CreateMap<CreateExchangeRateRequest, ExchangeRate>()
            .ForMember(dest => dest.BaseCurrencyId, opt => opt.Ignore())
            .ForMember(dest => dest.TargetCurrencyId, opt => opt.Ignore());
    }
}
