using CurrencyExchange.Models;
using CurrencyExchange.Repositories.Interfaces;
using Microsoft.Data.Sqlite;

namespace CurrencyExchange.Repositories;

public class CurrenciesRepository : ICurrenciesRepository
{
    private readonly string? _connectionString;

    public CurrenciesRepository(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection");
    }

    public List<Currency> GetAllCurrencies()
    {
        using var connection = new SqliteConnection(_connectionString);
        connection.Open();

        var command = connection.CreateCommand();
        command.CommandText =
        @"
            SELECT ID, Code, FullName, Sign
            FROM Currencies;
        ";

        using var reader = command.ExecuteReader();
        var currencies = new List<Currency>();
        while (reader.Read())
            currencies.Add(MapCurrency(reader));
        return currencies;
    }

    public Currency? GetCurrency(string code)
    {
        using var connection = new SqliteConnection(_connectionString);
        connection.Open();

        var command = connection.CreateCommand();
        command.CommandText =
        @"
            SELECT ID, Code, FullName, Sign
            FROM Currencies
            WHERE Code=@code;
        ";
        command.Parameters.AddWithValue("@code", code);

        using var reader = command.ExecuteReader();
        return reader.Read() ? MapCurrency(reader) : null;
    }

    public Currency AddCurrency(Currency currency)
    {
        using var connection = new SqliteConnection(_connectionString);
        connection.Open();

        var command = connection.CreateCommand();
        command.CommandText =
        @"
            INSERT INTO Currencies (Code, FullName, Sign)
            VALUES (@code, @fullName, @sign)
            RETURNING ID;
        ";
        command.Parameters.AddWithValue("@code", currency.Code);
        command.Parameters.AddWithValue("@fullName", currency.FullName);
        command.Parameters.AddWithValue("@sign", currency.Sign);

        var id = command.ExecuteScalar();
        return new Currency
        {
            Id = Convert.ToInt32(id),
            Code = currency.Code,
            FullName = currency.FullName,
            Sign = currency.Sign
        };
    }

    private static Currency MapCurrency(SqliteDataReader reader)
    {
        return new Currency
        {
            Id = reader.GetInt32(0),
            Code = reader.GetString(1),
            FullName = reader.GetString(2),
            Sign = reader.GetString(3)
        };
    }
}