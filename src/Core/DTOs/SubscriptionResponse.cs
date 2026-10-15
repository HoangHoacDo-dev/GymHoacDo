using System.Text.Json.Serialization;

namespace Core.DTOs;

public class SubscriptionResponse
{
    [JsonPropertyName("subscription_id")]
    public int SubscriptionId { get; set; }

    [JsonPropertyName("start_date")]
    public string StartDate { get; set; } = string.Empty;

    [JsonPropertyName("end_date")]
    public string EndDate { get; set; } = string.Empty;

    [JsonPropertyName("status")]
    public string Status { get; set; } = string.Empty;
}