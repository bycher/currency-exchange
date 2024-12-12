using CurrencyExchange.DataAccess;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")!;
builder.Services.AddSingleton(p => new CurrenciesRepository(connectionString));
builder.Services.AddSingleton(p => new ExchangeRatesRepository(
    connectionString, p.GetService<CurrenciesRepository>()!));
builder.Services.AddControllers();

var app = builder.Build();

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
