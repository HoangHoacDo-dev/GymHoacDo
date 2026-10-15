using System.Text.Json.Serialization;

namespace Core.DTOs;

public class CurrentSubscriptionDto
{
    [JsonPropertyName("subscription_id")]
public int SubscriptionId { get; set; }
    [JsonPropertyName("package_name")]
    public string PackageName { get; set; } = string.Empty;

    [JsonPropertyName("start_date")]
    public string StartDate { get; set; } = string.Empty;

    [JsonPropertyName("end_date")]
    public string EndDate { get; set; } = string.Empty;

    [JsonPropertyName("remaining_days")]
    public int RemainingDays { get; set; }

    [JsonPropertyName("status")]
    public string Status { get; set; } = string.Empty;
}