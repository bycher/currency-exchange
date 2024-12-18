using CurrencyExchange.Models;
using Microsoft.Data.Sqlite;

namespace CurrencyExchange.Repositories;

public abstract class RepositoryBase<T> where T : Entity {
    private readonly string _connectionString;

    public RepositoryBase(IConfiguration configuration) {
        _connectionString = configuration.GetConnectionString("DefaultConnection")!;
    }

    protected abstract T MapEntity(SqliteDataReader reader);

    protected IEnumerable<T> GetAllEntities(string query) {
        return ExecuteQuery(query, MapEntities);
    }

    protected T? GetEntity(
        string query, Action<SqliteCommand>? queryParamsConfigurator = null
    ) {
        return ExecuteQuery(
            query,
            reader => MapEntities(reader).FirstOrDefault(),
            queryParamsConfigurator
        );
    }

    protected T AddEntity(
        string query, T entity, Action<SqliteCommand>? queryParamsConfigurator = null
    ) {
        return ExecuteQuery(
            query,
            reader => (T)entity.CopyWithIdFromReader(reader)!,
            queryParamsConfigurator
        );
    }

    protected T? UpdateEntity(
        string query, T entity, Action<SqliteCommand>? queryParamsConfigurator = null
    ) {
        return ExecuteQuery(
            query,
            reader => (T?)entity.CopyWithIdFromReader(reader),
            queryParamsConfigurator
        );
    }

    private R ExecuteQuery<R>(
        string query,
        Func<SqliteDataReader, R> mapResult,
        Action<SqliteCommand>? configureCommandParameters = null
    ) {
        using var connection = new SqliteConnection(_connectionString);
        connection.Open();

        using var command = connection.CreateCommand();
        command.CommandText = query;
        configureCommandParameters?.Invoke(command);

        using var reader = command.ExecuteReader();

        return mapResult(reader);
    }

    private IEnumerable<T> MapEntities(SqliteDataReader reader) {
        var elements = new List<T>();

        while (reader.Read())
            elements.Add(MapEntity(reader));

        return elements;
    }
}