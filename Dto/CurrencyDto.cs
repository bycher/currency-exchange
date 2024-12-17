namespace CurrencyExchange.Dto;

public class CurrencyDto
{
    public required int Id { get; set; }
    public required string Code { get; set; }
    public required string FullName { get; set; }
    public required string Sign { get; set; }
}