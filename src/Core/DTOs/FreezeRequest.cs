using System;
using System.Text.Json.Serialization;

namespace Core.DTOs;

public class FreezeRequest
{
    [JsonPropertyName("start_date")]
    public DateTime StartDate { get; set; }

    [JsonPropertyName("end_date")]
    public DateTime EndDate { get; set; }

    [JsonPropertyName("reason")]
    public string Reason { get; set; } = string.Empty;
}