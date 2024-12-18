namespace CurrencyExchange.Models;

public record Currency : Entity {
    public string Code { get; init; } = null!;
    public string FullName { get; init; } = null!;
    public string Sign { get; init; } = null!;
}