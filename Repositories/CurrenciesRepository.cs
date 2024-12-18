using CurrencyExchange.Models;
using CurrencyExchange.Repositories.Interfaces;
using Microsoft.Data.Sqlite;

namespace CurrencyExchange.Repositories;

public sealed class CurrenciesRepository : RepositoryBase<Currency>, ICurrenciesRepository {
    public CurrenciesRepository(IConfiguration configuration) : base(configuration) {
    }

    public IEnumerable<Currency> GetAllCurrencies() {
        return GetAllEntities(
            @"
                SELECT ID, Code, FullName, Sign
                FROM Currencies;
            "
        );
    }

    public Currency? GetCurrency(string code) {
        return GetEntity(
            @"
                SELECT ID, Code, FullName, Sign
                FROM Currencies
                WHERE Code=@code;
            ",
            command => command.Parameters.AddWithValue("@code", code)
        );
    }

    public Currency AddCurrency(Currency currency) {
        return AddEntity(
            @"
                INSERT INTO Currencies (Code, FullName, Sign)
                VALUES (@code, @fullName, @sign)
                RETURNING ID;
            ",
            currency,
            command =>
            {
                command.Parameters.AddWithValue("@code", currency.Code);
                command.Parameters.AddWithValue("@fullName", currency.FullName);
                command.Parameters.AddWithValue("@sign", currency.Sign);
            }
        );
    }

    protected override Currency MapRow(SqliteDataReader reader) {
        return new Currency {
            Id = reader.GetInt32(0),
            Code = reader.GetString(1),
            FullName = reader.GetString(2),
            Sign = reader.GetString(3)
        };
    }
}