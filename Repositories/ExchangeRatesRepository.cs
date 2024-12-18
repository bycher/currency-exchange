using CurrencyExchange.Models;
using CurrencyExchange.Repositories.Interfaces;
using Microsoft.Data.Sqlite;

namespace CurrencyExchange.Repositories;

public sealed class ExchangeRatesRepository : RepositoryBase<ExchangeRate>, IExchangeRatesRepository {
    public ExchangeRatesRepository(IConfiguration configuration) : base(configuration) {
    }

    public List<ExchangeRate> GetAllExchangeRates() {
        return GetAllEntities(
            @"
                SELECT ID, BaseCurrencyId, TargetCurrencyId, Rate
                FROM ExchangeRates;
            "
        );
    }

    public ExchangeRate? GetExchangeRate(int baseCurrencyId, int targetCurrencyId) {
        return GetEntity(
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
    }

    public ExchangeRate AddExchangeRate(ExchangeRate exchangeRate) {
        return AddEntity(
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
    }

    public ExchangeRate? UpdateExchangeRate(ExchangeRate exchangeRate) {
        return UpdateEntity(
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
    }

    protected override ExchangeRate? MapRow(SqliteDataReader reader) {
        return reader.Read()
            ? new() {
                Id = reader.GetInt32(0),
                BaseCurrencyId = reader.GetInt32(1),
                TargetCurrencyId = reader.GetInt32(2),
                Rate = reader.GetDouble(3),
            }
            : null;
    }
}