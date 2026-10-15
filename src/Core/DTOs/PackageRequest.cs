using System.Text.Json.Serialization;

namespace Core.DTOs;

public class PackageRequest
{
    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;

    [JsonPropertyName("duration_in_days")]
    public int DurationInDays { get; set; }

    [JsonPropertyName("price")]
    public decimal Price { get; set; }
}
