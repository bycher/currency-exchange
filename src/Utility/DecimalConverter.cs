using System.Text.Json;
using System.Text.Json.Serialization;

namespace CurrencyExchange.Api.Utility;

public class DecimalConverter : JsonConverter<decimal> {
    private const int DecimalPlaces = 6;

    public override decimal Read(
        ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options
    ) {
        return reader.GetDecimal();
    }

    public override void Write(
        Utf8JsonWriter writer, decimal value, JsonSerializerOptions options
    ) {
        writer.WriteNumberValue(Math.Round(value, DecimalPlaces));
    }
}