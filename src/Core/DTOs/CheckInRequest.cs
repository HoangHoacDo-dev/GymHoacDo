using System.Text.Json.Serialization;

namespace Core.DTOs;

public class CheckInRequest
{
    [JsonPropertyName("phone_number")]
    public string PhoneNumber { get; set; } = string.Empty;
}


