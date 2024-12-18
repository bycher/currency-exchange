using Microsoft.Data.Sqlite;

namespace CurrencyExchange.Models;

public record Entity {
    public int Id { get; init; }

    public Entity? CopyWithIdFromReader(SqliteDataReader reader) {
        return reader.Read()
            ? this with { Id = reader.GetInt32(0) }
            : null;
    }
}