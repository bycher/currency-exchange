using CurrencyExchange.Models;
using Microsoft.Data.Sqlite;

namespace CurrencyExchange.DataAccess;

public class CurrenciesRepository
{
    private readonly string _connectionString;

    public CurrenciesRepository(string connectionString)
    {
        _connectionString = connectionString;
    }

    public List<Currency> GetAllCurrencies()
    {
        var currencies = new List<Currency>();
        
        using var connection = new SqliteConnection(_connectionString);
        connection.Open();
        
        var command = connection.CreateCommand();
        command.CommandText =
        @"
            SELECT ID, Code, FullName, Sign FROM Currencies;
        ";
        
        using var reader = command.ExecuteReader();
        while (reader.Read())
        {
            currencies.Add(new Currency
            {
                ID = reader.GetInt32(0),
                Code = reader.GetString(1),
                FullName = reader.GetString(2),
                Sign = reader.GetString(3)
            });
        }
        return currencies;
    }

    public Currency? GetCurrency(string code)
    {
        Currency? currency = null;

        using var connection = new SqliteConnection(_connectionString);
        connection.Open();
        
        var command = connection.CreateCommand();
        command.CommandText =
        @"
            SELECT ID, Code, FullName, Sign FROM Currencies
            WHERE Code=@code;
        ";
        command.Parameters.AddWithValue("@code", code);

        using var reader = command.ExecuteReader();
        if (reader.Read())
        {
            currency = new Currency
            {
                ID = reader.GetInt32(0),
                Code = reader.GetString(1),
                FullName = reader.GetString(2),
                Sign = reader.GetString(3)
            };
        }

        return currency;
    }
}