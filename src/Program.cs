using CurrencyExchange.Api.Data;
using CurrencyExchange.Api.Interfaces;
using CurrencyExchange.Api.Mapping;
using CurrencyExchange.Api.Middlewares;
using CurrencyExchange.Api.Services;
using CurrencyExchange.Api.Validation;

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

builder.Services.AddCors(opt => opt.AddDefaultPolicy(
    builder => builder
        .AllowAnyOrigin()
        .AllowAnyMethod()
        .AllowAnyHeader()
    )
);

var app = builder.Build();

if (app.Environment.IsProduction()) {
    app.UseHsts();
}
if (app.Environment.IsDevelopment()) {
    app.UseCors();
}
app.UseMiddleware<ExceptionHandlingMiddleware>();
app.UseHttpsRedirection();
app.MapControllers();

app.Run();