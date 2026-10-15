using System.Text.Json.Serialization;

namespace Core.DTOs;

public class ExpiringMemberDto
{
    [JsonPropertyName("member_name")]
    public string MemberName { get; set; } = string.Empty;

    [JsonPropertyName("phone_number")]
    public string PhoneNumber { get; set; } = string.Empty;

    [JsonPropertyName("package_name")]
    public string PackageName { get; set; } = string.Empty;

    [JsonPropertyName("end_date")]
    public string EndDate { get; set; } = string.Empty;

    [JsonPropertyName("remaining_days")]
    public int RemainingDays { get; set; }
}