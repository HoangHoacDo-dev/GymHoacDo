using System.Text.Json.Serialization;

namespace Core.DTOs;

public class CheckInResponse
{
    [JsonPropertyName("status")]
    public string Status { get; set; } = "success";

    [JsonPropertyName("message")]
    public string Message { get; set; } = string.Empty;

    [JsonPropertyName("checkin_time")]
    public string CheckinTime { get; set; } = string.Empty;
}
