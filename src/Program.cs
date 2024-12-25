using CurrencyExchange.Api.Data;
using CurrencyExchange.Api.Interfaces;
using CurrencyExchange.Api.Mapping;
using CurrencyExchange.Api.Middlewares;
using CurrencyExchange.Api.Services;
using CurrencyExchange.Api.Utility;
using CurrencyExchange.Api.Validation;
using Microsoft.AspNetCore.Localization;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<DatabaseInitializer>();

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
    .AddJsonOptions(opt => opt.JsonSerializerOptions.Converters.Add(new DecimalConverter()))
    .ConfigureApiBehaviorOptions(opt => opt.SuppressModelStateInvalidFilter = true);

// To test API from frontend (in development)
if (builder.Environment.IsDevelopment()) {
    builder.Services.AddCors(opt => opt.AddDefaultPolicy(
        builder => builder
            .AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader()
    ));
}

var app = builder.Build();

var databaseInitializer = app.Services.GetRequiredService<DatabaseInitializer>();
databaseInitializer.EnsureCreated();

app.UseMiddleware<ExceptionHandlingMiddleware>();

// To use decimal point as a separator
app.UseRequestLocalization(new RequestLocalizationOptions {
    DefaultRequestCulture = new RequestCulture("en-US")
});
if (app.Environment.IsProduction()) {
    app.UseHsts();
}
if (app.Environment.IsDevelopment()) {
    app.UseCors();
    app.UseHttpsRedirection();
}
app.MapControllers();

app.Run();