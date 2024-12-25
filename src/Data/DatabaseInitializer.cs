using CurrencyExchange.Api.Exceptions;
using Microsoft.Data.Sqlite;

/// <summary>
/// Initializes the database.
/// </summary>
public class DatabaseInitializer {
    /// <summary>
    /// The connection string to the database.
    /// </summary>
    private readonly string _connectionString;

    /// <summary>
    /// Initializes a new instance of the <see cref="DatabaseInitializer"/> class.
    /// </summary>
    /// <param name="configuration">The configuration.</param>
    /// <param name="connectionString">The connection string.</param>
    /// <exception cref="ArgumentException">Thrown when the connection string is not configured.</exception>
    public DatabaseInitializer(IConfiguration configuration, string connectionString = "DefaultConnection") {
        _connectionString = configuration.GetConnectionString(connectionString)
            ?? throw new ArgumentException(
                $"{connectionString} string is not configured", nameof(configuration)
            );
    }

    /// <summary>
    /// Ensures that the database is created.
    /// </summary>
    /// <exception cref="DatabaseException">Thrown when failed to initialize database.</exception>
    public void EnsureCreated() {
        try {
            string databasePath = new SqliteConnectionStringBuilder(_connectionString).DataSource;
            if (!File.Exists(databasePath))
                Initialize();
        }
        catch (Exception ex) {
            throw new DatabaseException("Failed to initialize database", ex);
        }
    }

    /// <summary>
    /// Initializes the database.
    /// </summary>
    private void Initialize() {
        using var connection = new SqliteConnection(_connectionString);
        connection.Open();

        using var command = connection.CreateCommand();
        command.CommandText =
        @"
            CREATE TABLE IF NOT EXISTS Currencies (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                Code VARCHAR UNIQUE,
                FullName VARCHAR,
                Sign VARCHAR
            );

            CREATE TABLE IF NOT EXISTS ExchangeRates (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                BaseCurrencyId INTEGER,
                TargetCurrencyId INTEGER,
                Rate DECIMAL(6),
                FOREIGN KEY (BaseCurrencyId) REFERENCES Currencies(Id),
                FOREIGN KEY (TargetCurrencyId) REFERENCES Currencies(Id),
                UNIQUE (BaseCurrencyId, TargetCurrencyId)
            );
        ";
        command.ExecuteNonQuery();
    }
}
