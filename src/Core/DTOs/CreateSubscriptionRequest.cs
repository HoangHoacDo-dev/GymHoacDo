using System.Text.Json.Serialization;

namespace Core.DTOs;

public class CreateSubscriptionRequest
{
    [JsonPropertyName("member_id")]
    public int MemberId { get; set; }

    [JsonPropertyName("package_id")]
    public int PackageId { get; set; }

    [JsonPropertyName("employee_id")]
    public int EmployeeId { get; set; }
}