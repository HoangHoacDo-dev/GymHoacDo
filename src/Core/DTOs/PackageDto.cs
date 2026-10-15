using System.Text.Json.Serialization;

namespace Core.DTOs;

public class PackageDto
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;

    [JsonPropertyName("duration_in_days")]
    public int DurationInDays { get; set; }

    [JsonPropertyName("price")]
    public decimal Price { get; set; }
}
