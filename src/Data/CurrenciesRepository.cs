using CurrencyExchange.Api.Interfaces;
using CurrencyExchange.Api.Models.Entities;
using Microsoft.Data.Sqlite;

namespace CurrencyExchange.Api.Data;

/// <summary>
/// Repository for managing currencies. It represents a data access layer for currencies.
/// </summary>
/// <param name="configuration">Application configuration.</param>
public sealed class CurrenciesRepository(IConfiguration configuration)
    : RepositoryBase<Currency>(configuration), ICurrenciesRepository {
    /// <summary>
    /// Gets all currencies.
    /// </summary>
    /// <returns>Currencies collection.</returns>
    public IEnumerable<Currency> GetAllCurrencies() => GetAllEntities(
        @"
            SELECT ID, Code, FullName, Sign
            FROM Currencies;
        "
    );

    /// <summary>
    /// Gets a currency by its code.
    /// </summary>
    /// <param name="code">Currency code.</param>
    /// <returns>Currency.</returns>
    public Currency? GetCurrency(string code) => GetEntity(
        @"
            SELECT ID, Code, FullName, Sign
            FROM Currencies
            WHERE Code=@code;
        ",
        command => command.Parameters.AddWithValue("@code", code)
    );

    /// <summary>
    /// Adds a currency.
    /// </summary>
    /// <param name="currency">Currency.</param>
    /// <returns>Added currency.</returns>
    public Currency AddCurrency(Currency currency) => AddEntity(
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

    /// <summary>
    /// Maps a currency from a reader.
    /// </summary>
    /// <param name="reader">Sqlite reader.</param>
    /// <returns>Mapped currency.</returns>
    protected override Currency MapEntity(SqliteDataReader reader) => new() {
        Id = reader.GetInt32(0),
        Code = reader.GetString(1),
        FullName = reader.GetString(2),
        Sign = reader.GetString(3)
    };
}