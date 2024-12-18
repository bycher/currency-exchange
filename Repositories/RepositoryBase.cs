using CurrencyExchange.Models;
using Microsoft.Data.Sqlite;

namespace CurrencyExchange.Repositories;

public abstract class RepositoryBase<T> where T : Entity {
    private readonly string _connectionString;

    public RepositoryBase(IConfiguration configuration) {
        _connectionString = configuration.GetConnectionString("DefaultConnection")!;
    }

    protected List<T> GetAllEntities(string query) {
        return ExecuteQuery(query, MapRows);
    }

    protected T? GetEntity(
        string query, Action<SqliteCommand>? configureCommandParameters = null
    ) {
        return ExecuteQuery(query, MapRow, configureCommandParameters);
    }

    protected T AddEntity(
        string query, T entityData, Action<SqliteCommand>? configureCommandParameters = null
    ) {
        return ExecuteQuery(
            query,
            reader => MapWithDefinedId(reader, entityData)!,
            configureCommandParameters
        );
    }

    protected T? UpdateEntity(
        string query, T newEntityData, Action<SqliteCommand>? configureCommandParameters = null
    ) {
        return ExecuteQuery(
            query,
            reader => MapWithDefinedId(reader, newEntityData),
            configureCommandParameters
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

    private static T? MapWithDefinedId(SqliteDataReader reader, T entity) {
        return entity != null
            ? entity with { Id = reader.GetInt32(0) }
            : null;
    }

    protected abstract T? MapRow(SqliteDataReader reader);

    private  List<T> MapRows(SqliteDataReader reader) {
        var elements = new List<T>();

        while (reader.Read())
            elements.Add(MapRow(reader)!);

        return elements;
    }
}