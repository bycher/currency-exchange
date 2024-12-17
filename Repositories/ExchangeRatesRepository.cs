using CurrencyExchange.Models;
using CurrencyExchange.Repositories.Interfaces;
using Microsoft.Data.Sqlite;

namespace CurrencyExchange.Repositories;

public class ExchangeRatesRepository : IExchangeRatesRepository
{
    private readonly string? _connectionString;

    public ExchangeRatesRepository(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection");
    }

    public List<ExchangeRate> GetAllExchangeRates()
    {
        var exchangeRates = new List<ExchangeRate>();

        using var connection = new SqliteConnection(_connectionString);
        connection.Open();

        var command = connection.CreateCommand();
        command.CommandText =
        @"
            SELECT ID, BaseCurrencyId, TargetCurrencyId, Rate
            FROM ExchangeRates;
        ";

        using var reader = command.ExecuteReader();
        while (reader.Read())
            exchangeRates.Add(MapExchangeRate(reader));

        return exchangeRates;
    }

    public ExchangeRate? GetExchangeRate(int baseCurrencyId, int targetCurrencyId)
    {
        using var connection = new SqliteConnection(_connectionString);
        connection.Open();

        var command = connection.CreateCommand();
        command.CommandText =
        @"
            SELECT ID, BaseCurrencyId, TargetCurrencyId, Rate
            FROM ExchangeRates
            WHERE BaseCurrencyId = @baseCurrencyId AND TargetCurrencyId = @targetCurrencyId;
        ";
        command.Parameters.AddWithValue("@baseCurrencyId", baseCurrencyId);
        command.Parameters.AddWithValue("@targetCurrencyId", targetCurrencyId);

        using var reader = command.ExecuteReader();
        return reader.Read() ? MapExchangeRate(reader) : null;
    }

    public ExchangeRate AddExchangeRate(ExchangeRate exchangeRate)
    {
        using var connection = new SqliteConnection(_connectionString);
        connection.Open();

        var command = connection.CreateCommand();
        command.CommandText =
        @"
            INSERT INTO ExchangeRates (BaseCurrencyId, TargetCurrencyId, Rate)
            VALUES (@baseCurrencyId, @targetCurrencyId, @rate)
            RETURNING ID; 
        ";
        command.Parameters.AddWithValue("@baseCurrencyId", exchangeRate.BaseCurrencyId);
        command.Parameters.AddWithValue("@targetCurrencyId", exchangeRate.TargetCurrencyId);
        command.Parameters.AddWithValue("@rate", exchangeRate.Rate);

        var insertedRowId = command.ExecuteScalar();

        return new ExchangeRate
        {
            Id = Convert.ToInt32(insertedRowId),
            BaseCurrencyId = exchangeRate.BaseCurrencyId,
            TargetCurrencyId = exchangeRate.TargetCurrencyId,
            Rate = exchangeRate.Rate
        };
    }

    public ExchangeRate? UpdateExchangeRate(int baseCurrencyId, int targetCurrencyId, double newRate)
    {
        using var connection = new SqliteConnection(_connectionString);
        connection.Open();

        var command = connection.CreateCommand();
        command.CommandText =
        @"
            UPDATE ExchangeRates
            SET Rate = @rate
            WHERE BaseCurrencyId = @baseCurrencyId AND TargetCurrencyId = @targetCurrencyId
            RETURNING ID;
        ";
        command.Parameters.AddWithValue("@rate", newRate);
        command.Parameters.AddWithValue("@baseCurrencyId", baseCurrencyId);
        command.Parameters.AddWithValue("@targetCurrencyId", targetCurrencyId);

        var updatedID = command.ExecuteScalar();
        if (updatedID == null)
            return null;

        return new ExchangeRate
        {
            Id = Convert.ToInt32(updatedID),
            BaseCurrencyId = baseCurrencyId,
            TargetCurrencyId = targetCurrencyId,
            Rate = newRate
        };
    }

    private static ExchangeRate MapExchangeRate(SqliteDataReader reader)
    {
        return new ExchangeRate
        {
            Id = reader.GetInt32(0),
            BaseCurrencyId = reader.GetInt32(1),
            TargetCurrencyId = reader.GetInt32(2),
            Rate = reader.GetDouble(3),
        };
    }
}