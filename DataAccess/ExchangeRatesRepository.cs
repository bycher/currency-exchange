using CurrencyExchange.Models;
using Microsoft.Data.Sqlite;

namespace CurrencyExchange.DataAccess;

public class ExchangeRatesRepository
{
    private readonly string _connectionString;
    private readonly CurrenciesRepository _currenciesRepository;

    public ExchangeRatesRepository(string connectionString, CurrenciesRepository currenciesRepository)
    {
        _connectionString = connectionString;
        _currenciesRepository = currenciesRepository;
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

    public ExchangeRate? AddExchangeRate(ExchangeRateForm exchangeRateForm)
    {
        using var connection = new SqliteConnection(_connectionString);
        connection.Open();

        var command = connection.CreateCommand();
        command.CommandText =
        @"
            INSERT INTO ExchangeRates (BaseCurrencyId, TargetCurrencyId, Rate)
            SELECT c1.ID, c2.ID, @rate
            FROM Currencies c1
            INNER JOIN Currencies c2 ON c1.ID != c2.ID
            WHERE c1.Code = @baseCurrencyCode AND c2.Code = @targetCurrencyCode
            RETURNING ID; 
        ";
        command.Parameters.AddWithValue("@rate", exchangeRateForm.Rate);
        command.Parameters.AddWithValue("@baseCurrencyCode", exchangeRateForm.BaseCurrencyCode);
        command.Parameters.AddWithValue("@targetCurrencyCode", exchangeRateForm.TargetCurrencyCode);
        
        var insertedRowId = command.ExecuteScalar();
        connection.Close();

        var baseCurrency = _currenciesRepository.GetCurrency(exchangeRateForm.BaseCurrencyCode)
            ?? throw new InvalidOperationException(
                $"Currency with code '{exchangeRateForm.BaseCurrencyCode}' wasn't found in database");
        var targetCurrency = _currenciesRepository.GetCurrency(exchangeRateForm.TargetCurrencyCode)
            ?? throw new InvalidOperationException(
                $"Currency with code '{exchangeRateForm.TargetCurrencyCode}' wasn't found in database");

        return new ExchangeRate
        {
            ID = Convert.ToInt32(insertedRowId),
            BaseCurrency = baseCurrency,
            TargetCurrency = targetCurrency,
            Rate = exchangeRateForm.Rate
        };
    }

    public ExchangeRate? UpdateExchangeRate(string baseCode, string targetCode, double newRate)
    {   
        var baseCurrency = _currenciesRepository.GetCurrency(baseCode);
        if (baseCurrency == null)
            return null;

        var targetCurrency = _currenciesRepository.GetCurrency(targetCode);
        if (targetCurrency == null)
            return null;

        using var connection = new SqliteConnection(_connectionString);
        connection.Open();

        var command = connection.CreateCommand();
        command.CommandText =
        @"
            UPDATE ExchangeRates
            SET Rate = @rate
            WHERE BaseCurrencyId = @baseId AND TargetCurrencyId = @targetId
            RETURNING ID;
        ";
        command.Parameters.AddWithValue("@rate", newRate);
        command.Parameters.AddWithValue("@baseId", baseCurrency.ID);
        command.Parameters.AddWithValue("@targetId", targetCurrency.ID);

        var updatedID = command.ExecuteScalar();
        if (updatedID == null)
            return null;
        
        return new ExchangeRate
        {
            ID = Convert.ToInt32(updatedID),
            BaseCurrency = baseCurrency,
            TargetCurrency = targetCurrency,
            Rate = newRate
        };
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