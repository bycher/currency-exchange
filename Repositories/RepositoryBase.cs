using CurrencyExchange.Exceptions;
using CurrencyExchange.Models.Domain;
using Microsoft.Data.Sqlite;

namespace CurrencyExchange.Repositories;

/// <summary>
/// Base class for repositories. It provides common functionality for all operations with database.
/// </summary>
/// <typeparam name="T">Entity type.</typeparam>
public abstract class RepositoryBase<T> where T : Entity {
    /// <summary>
    /// Unique constraint error code (SQLite).
    /// </summary>
    private const int UniqueConstraintErrorCode = 19;

    /// <summary>
    /// Connection string.
    /// </summary>
    private readonly string _connectionString;

    /// <summary>
    /// Initializes a new instance of the <see cref="RepositoryBase{T}"/> class.
    /// </summary>
    /// <param name="configuration">Application configuration.</param>
    /// <param name="connectionString">Connection string.</param>
    public RepositoryBase(IConfiguration configuration, string connectionString = "DefaultConnection") {
        _connectionString = configuration.GetConnectionString(connectionString)
            ?? throw new ArgumentException(
                $"{connectionString} string is not configured", nameof(configuration)
            );
    }

    /// <summary>
    /// Gets all entities.
    /// </summary>
    /// <param name="query">SQL query.</param>
    /// <returns>All entities.</returns>
    protected IEnumerable<T> GetAllEntities(string query) =>
        ExecuteQuery(query, MapEntities);

    /// <summary>
    /// Gets an entity
    /// </summary>
    /// <param name="query">SQL query.</param>
    /// <param name="queryParamsConfigurator">Query params configurator.</param>
    /// <returns>Entity.</returns>
    protected T? GetEntity(string query, Action<SqliteCommand> queryParamsConfigurator) =>
        ExecuteQuery(
            query,
            reader => MapEntities(reader).FirstOrDefault(),
            queryParamsConfigurator
        );

    /// <summary>
    /// Adds a new entity.
    /// </summary>
    /// <param name="query">SQL query.</param>
    /// <param name="entity">Entity to add.</param>
    /// <param name="queryParamsConfigurator">Query params configurator.</param>
    /// <returns>Added entity.</returns>
    /// <exception cref="DatabaseConflictException">Thrown when entity already exists.</exception>
    protected T AddEntity(string query, T entity, Action<SqliteCommand> queryParamsConfigurator) {
        try {
            return ExecuteQuery(
                query,
                reader => CopyWithIdFromReader(entity, reader)!,
                queryParamsConfigurator
            );
        }
        catch (DatabaseException ex) when (IsUniqueConstraintViolated(ex)) {
            throw new DatabaseConflictException($"{typeof(T).Name} already exists in database", ex);
        }
    }

    /// <summary>
    /// Updates an existing entity.
    /// </summary>
    /// <param name="query">SQL query.</param>
    /// <param name="entity">Entity to update.</param>
    /// <param name="queryParamsConfigurator">Query params configurator.</param>
    /// <returns>Updated entity.</returns>
    protected T? UpdateEntity(string query, T entity, Action<SqliteCommand> queryParamsConfigurator) =>
        ExecuteQuery(
            query,
            reader => CopyWithIdFromReader(entity, reader),
            queryParamsConfigurator
        );

    /// <summary>
    /// Executes an arbitrary SQL query, configures its params and returns the mapped result.
    /// </summary>
    /// <typeparam name="R">Result type.</typeparam>
    /// <param name="query">SQL query.</param>
    /// <param name="queryResultMapper">Query result mapper.</param>
    /// <param name="queryParamsConfigurator">Query params configurator.</param>
    /// <returns>Query result.</returns>
    /// <exception cref="DatabaseException">Thrown when database error occurs.</exception>
    private R ExecuteQuery<R>(
        string query,
        Func<SqliteDataReader, R> queryResultMapper,
        Action<SqliteCommand>? queryParamsConfigurator = null
    ) {
        try {
            using var connection = new SqliteConnection(_connectionString);
            connection.Open();

            using var command = connection.CreateCommand();
            command.CommandText = query;
            queryParamsConfigurator?.Invoke(command);

            using var reader = command.ExecuteReader();

            return queryResultMapper(reader);
        }
        catch (Exception ex) {
            throw new DatabaseException("Error occurred while performing query", ex);
        }
    }

    /// <summary>
    /// Maps an entity from a reader.
    /// </summary>
    /// <param name="reader">Reader.</param>
    /// <returns>Mapped entity.</returns>
    protected abstract T MapEntity(SqliteDataReader reader);

    /// <summary>
    /// Maps a list of entities from a reader.
    /// </summary>
    /// <param name="reader">Sqlite reader.</param>
    /// <returns>Mapped entities.</returns>
    private IEnumerable<T> MapEntities(SqliteDataReader reader) {
        var elements = new List<T>();

        while (reader.Read())
            elements.Add(MapEntity(reader));

        return elements;
    }

    /// <summary>
    /// Copies an entity with ID from a reader.
    /// </summary>
    /// <param name="entity">Entity.</param>
    /// <param name="reader">Sqlite reader.</param>
    /// <returns>Entity with ID.</returns>
    private static T? CopyWithIdFromReader(T entity, SqliteDataReader reader) {
        entity.Id = reader.GetInt32(0);
        return entity;
    }

    /// <summary>
    /// Checks if a database exception is due to a unique constraint violation.
    /// </summary>
    /// <param name="ex">Database exception.</param>
    /// <returns>True if the exception is due to a unique constraint violation, false otherwise.</returns>
    private static bool IsUniqueConstraintViolated(Exception ex) =>
        ex.InnerException is SqliteException sqliteEx &&
        sqliteEx.SqliteErrorCode == UniqueConstraintErrorCode;
}
