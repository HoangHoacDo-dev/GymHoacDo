using System.Text.Json.Serialization;

namespace Core.DTOs
{
    public class LoginResponse
    {
        [JsonPropertyName("status")]
        public string Status { get; set; } = "success";
        [JsonPropertyName("message")]
        public string Message { get; set; } = string.Empty;

        [JsonPropertyName("data")]
        public LoginData Data { get; set; } = new LoginData();
        
    }

    public class LoginData
    {
        [JsonPropertyName("token")]
        public string Token { get; set; } = string.Empty;

        [JsonPropertyName("role")]
        public string Role { get; set; } = string.Empty;

        [JsonPropertyName("employee_id")]
        public int EmployeeId { get; set; }
        [JsonPropertyName("member_id")]
        public int MemberId { get; set; }

        [JsonPropertyName("full_name")]
        public string FullName { get; set; } = string.Empty;
    }
}