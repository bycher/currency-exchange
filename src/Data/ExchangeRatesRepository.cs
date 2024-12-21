using CurrencyExchange.Api.Interfaces;
using CurrencyExchange.Api.Models.Entities;
using Microsoft.Data.Sqlite;

namespace CurrencyExchange.Api.Data;

/// <summary>
/// Repository for managing exchange rates. It represents a data access layer for exchange rates.
/// </summary>
/// <param name="configuration">Application configuration.</param>
public sealed class ExchangeRatesRepository(IConfiguration configuration)
    : RepositoryBase<ExchangeRate>(configuration), IExchangeRatesRepository {  
    /// <summary>
    /// Gets all exchange rates.
    /// </summary>
    /// <returns>Exchange rates collection.</returns>
    public IEnumerable<ExchangeRate> GetAllExchangeRates() => GetAllEntities(
        @"
            SELECT ID, BaseCurrencyId, TargetCurrencyId, Rate
            FROM ExchangeRates;
        "
    );

    /// <summary>
    /// Gets an exchange rate for a given currency pair.
    /// </summary>
    /// <param name="baseCurrencyId">Base currency ID.</param>
    /// <param name="targetCurrencyId">Target currency ID.</param>
    /// <returns>Exchange rate.</returns>
    public ExchangeRate? GetExchangeRate(int baseCurrencyId, int targetCurrencyId) => GetEntity(
        @"
            SELECT ID, BaseCurrencyId, TargetCurrencyId, Rate
            FROM ExchangeRates
            WHERE BaseCurrencyId = @baseCurrencyId AND TargetCurrencyId = @targetCurrencyId;
        ",
        command => {
            command.Parameters.AddWithValue("@baseCurrencyId", baseCurrencyId);
            command.Parameters.AddWithValue("@targetCurrencyId", targetCurrencyId);
        }
    );

    /// <summary>
    /// Adds an exchange rate.
    /// </summary>
    /// <param name="exchangeRate">Exchange rate.</param>
    /// <returns>Added exchange rate.</returns>
    public ExchangeRate AddExchangeRate(ExchangeRate exchangeRate) => AddEntity(
        @"
            INSERT INTO ExchangeRates (BaseCurrencyId, TargetCurrencyId, Rate)
            VALUES (@baseCurrencyId, @targetCurrencyId, @rate)
            RETURNING ID; 
        ",
        exchangeRate,
        command => {
            command.Parameters.AddWithValue("@rate", exchangeRate.Rate);
            command.Parameters.AddWithValue("@baseCurrencyId", exchangeRate.BaseCurrencyId);
            command.Parameters.AddWithValue("@targetCurrencyId", exchangeRate.TargetCurrencyId);
        }
    );

    /// <summary>
    /// Updates an exchange rate.
    /// </summary>
    /// <param name="exchangeRate">Exchange rate.</param>
    /// <returns>Updated exchange rate.</returns>
    public ExchangeRate? UpdateExchangeRate(ExchangeRate exchangeRate) => UpdateEntity(
        @"
            UPDATE ExchangeRates
            SET Rate = @rate
            WHERE BaseCurrencyId = @baseCurrencyId AND TargetCurrencyId = @targetCurrencyId
            RETURNING ID;
        ",
        exchangeRate,
        command =>
        {
            command.Parameters.AddWithValue("@rate", exchangeRate.Rate);
            command.Parameters.AddWithValue("@baseCurrencyId", exchangeRate.BaseCurrencyId);
            command.Parameters.AddWithValue("@targetCurrencyId", exchangeRate.TargetCurrencyId);
        }
    );

    /// <summary>
    /// Maps an exchange rate from a reader.
    /// </summary>
    /// <param name="reader">Sqlite reader.</param>
    /// <returns>Mapped exchange rate.</returns>
    protected override ExchangeRate MapEntity(SqliteDataReader reader) => new() {
        Id = reader.GetInt32(0),
        BaseCurrencyId = reader.GetInt32(1),
        TargetCurrencyId = reader.GetInt32(2),
        Rate = reader.GetDouble(3),
    };
}
