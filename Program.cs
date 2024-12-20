using CurrencyExchange.Mapping;
using CurrencyExchange.Middlewares;
using CurrencyExchange.Repositories;
using CurrencyExchange.Repositories.Interfaces;
using CurrencyExchange.Services;
using CurrencyExchange.Services.Interfaces;
using CurrencyExchange.Validation;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAutoMapper(config => {
    config.AddProfile<CurrencyProfile>();
    config.AddProfile<ExchangeRateProfile>();
});

// Configure repositories
builder.Services.AddScoped<ICurrenciesRepository, CurrenciesRepository>();
builder.Services.AddScoped<IExchangeRatesRepository, ExchangeRatesRepository>();

// Configure services
builder.Services.AddScoped<ICurrencyService, CurrencyService>();
builder.Services.AddScoped<IExchangeRateService, ExchangeRateService>();
builder.Services.AddScoped<IExchangeService, ExchangeService>();

builder.Services
    .AddControllers(opt => opt.Filters.Add<ValidateModelFilter>())
    .ConfigureApiBehaviorOptions(opt => opt.SuppressModelStateInvalidFilter = true);  

var app = builder.Build();

app.UseMiddleware<ExceptionHandlingMiddleware>();
app.UseHttpsRedirection();
app.MapControllers();

app.Run();