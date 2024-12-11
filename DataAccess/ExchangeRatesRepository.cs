using CurrencyExchange.Models;
using Microsoft.Data.Sqlite;

namespace CurrencyExchange.DataAccess;

public class ExchangeRatesRepository
{
    private readonly string _connectionString;

    public ExchangeRatesRepository(string connectionString)
    {
        _connectionString = connectionString;
    }

    public List<ExchangeRate> GetAllExchangeRates()
    {
        var exchangeRates = new List<ExchangeRate>();

        using var connection = new SqliteConnection(_connectionString);
        connection.Open();

        var command = connection.CreateCommand();
        command.CommandText =
        @"
            SELECT 
                er.ID, 
                bc.ID, bc.Code, bc.FullName, bc.Sign, 
                tc.ID, tc.Code, tc.FullName, tc.Sign, 
                er.Rate
            FROM ExchangeRates AS er
            INNER JOIN Currencies AS bc ON er.BaseCurrencyId = bc.ID
            INNER JOIN Currencies AS tc ON er.TargetCurrencyId = tc.ID;
        ";

        using var reader = command.ExecuteReader();
        while (reader.Read())
            exchangeRates.Add(MapExchangeRate(reader));

        return exchangeRates;
    }

    public ExchangeRate? GetExchangeRate(string baseCode, string targetCode)
    {
        using var connection = new SqliteConnection(_connectionString);
        connection.Open();

        var command = connection.CreateCommand();
        command.CommandText =
        @"
            SELECT 
                er.ID, 
                bc.ID, bc.Code, bc.FullName, bc.Sign, 
                tc.ID, tc.Code, tc.FullName, tc.Sign, 
                er.Rate
            FROM ExchangeRates AS er
            INNER JOIN Currencies AS bc ON er.BaseCurrencyId = bc.ID AND bc.Code = @baseCode
            INNER JOIN Currencies AS tc ON er.TargetCurrencyId = tc.ID AND tc.Code = @targetCode;
        ";
        command.Parameters.AddWithValue("@baseCode", baseCode);
        command.Parameters.AddWithValue("@targetCode", targetCode);

        using var reader = command.ExecuteReader();
        return reader.Read() ? MapExchangeRate(reader) : null;
    }

    private static ExchangeRate MapExchangeRate(SqliteDataReader reader)
    {
        return new ExchangeRate
        {
            ID = reader.GetInt32(0),
            BaseCurrency = new Currency
            {
                ID = reader.GetInt32(1),
                Code = reader.GetString(2),
                FullName = reader.GetString(3),
                Sign = reader.GetString(4)
            },
            TargetCurrency = new Currency
            {
                ID = reader.GetInt32(5),
                Code = reader.GetString(6),
                FullName = reader.GetString(7),
                Sign = reader.GetString(8)
            },
            Rate = reader.GetDouble(9)
        };
    }
}