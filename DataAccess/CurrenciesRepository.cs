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
            SELECT ID, Code, FullName, Sign
            FROM Currencies;
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
            SELECT ID, Code, FullName, Sign
            FROM Currencies
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

    public Currency? AddCurrency(Currency currency)
    {
        using var connection = new SqliteConnection(_connectionString);
        connection.Open();

        var selectCommand = connection.CreateCommand();
        selectCommand.CommandText =
        @"
            SELECT ID
            FROM Currencies
            WHERE Code=@code;
        ";
        selectCommand.Parameters.AddWithValue("@code", currency.Code);

        var id = selectCommand.ExecuteScalar();
        if (id != null)
            return null;
        
        var insertCommand = connection.CreateCommand();
        insertCommand.CommandText =
        @"
            INSERT INTO Currencies (Code, FullName, Sign)
            VALUES (@code, @fullName, @sign)
            RETURNING ID;
        ";
        insertCommand.Parameters.AddWithValue("@code", currency.Code);
        insertCommand.Parameters.AddWithValue("@fullName", currency.FullName);
        insertCommand.Parameters.AddWithValue("@sign", currency.Sign);

        id = insertCommand.ExecuteScalar()!;

        return new Currency
        {
            ID = Convert.ToInt32(id),
            Code = currency.Code,
            FullName = currency.FullName,
            Sign = currency.Sign
        };
    }
}