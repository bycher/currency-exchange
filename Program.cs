using CurrencyExchange.Repositories;
using CurrencyExchange.Repositories.Interfaces;
using CurrencyExchange.Services;
using CurrencyExchange.Services.Interfaces;

internal class Program {
    private static void Main(string[] args) {
        var builder = WebApplication.CreateBuilder(args);
        
        ConfigureServices(builder);    

        var app = builder.Build();

        app.UseHttpsRedirection();
        app.UseAuthorization();
        app.MapControllers();

        app.Run();
    }

    private static void ConfigureServices(WebApplicationBuilder builder) {
        builder.Services.AddAutoMapper(typeof(Program));

        // Configure repositories
        builder.Services.AddScoped<ICurrenciesRepository, CurrenciesRepository>();
        builder.Services.AddScoped<IExchangeRatesRepository, ExchangeRatesRepository>();

        // Configure services
        builder.Services.AddScoped<ICurrencyService, CurrencyService>();
        builder.Services.AddScoped<IExchangeRateService, ExchangeRateService>();
        builder.Services.AddScoped<IExchangeService, ExchangeService>();

        builder.Services.AddControllers();
    }
}