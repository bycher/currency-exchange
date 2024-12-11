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
                exchangeRates.ID AS ExchangeRateId, 
                baseCurrency.ID AS BaseCurrencyId, 
                baseCurrency.Code AS BaseCurrencyCode, 
                baseCurrency.FullName AS BaseCurrencyName, 
                baseCurrency.Sign AS BaseCurrencySign, 
                targetCurrency.ID AS TargetCurrencyId, 
                targetCurrency.Code AS TargetCurrencyCode, 
                targetCurrency.FullName AS TargetCurrencyName, 
                targetCurrency.Sign AS TargetCurrencySign, 
                exchangeRates.Rate
            FROM ExchangeRates AS exchangeRates
            INNER JOIN Currencies AS baseCurrency
                ON exchangeRates.BaseCurrencyId = baseCurrency.ID
            INNER JOIN Currencies AS targetCurrency 
                ON exchangeRates.TargetCurrencyId = targetCurrency.ID;
        ";

        using var reader = command.ExecuteReader();
        while (reader.Read())
        {
            exchangeRates.Add(new ExchangeRate
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
            });
        }

        return exchangeRates;
    }
}