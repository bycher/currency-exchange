using CurrencyExchange.DataAccess;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton(p =>
{
    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
    return new CurrenciesRepository(connectionString!);
});
builder.Services.AddControllers();

var app = builder.Build();

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
