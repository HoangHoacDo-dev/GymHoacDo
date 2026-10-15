using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Core.DTOs;

public class CreateMemberRequest
{
    [Required(ErrorMessage = "Tên hội viên không được để trống")]
    [JsonPropertyName("full_name")]
    public required string FullName { get; set; }

    [Required(ErrorMessage = "Số điện thoại không được để trống")]
    [Phone(ErrorMessage = "Số điện thoại không đúng định dạng")]
    [JsonPropertyName("phone_number")]
    public required string PhoneNumber { get; set; }

    [Required(ErrorMessage = "Ngày sinh không được để trống")]
    [JsonPropertyName("dob")]
    public required DateOnly Dob { get; set; } 

    [Required(ErrorMessage = "Giới tính không được để trống")]
    [JsonPropertyName("gender")]
    public required string Gender { get; set; }

    [Required(ErrorMessage = "Địa chỉ không được để trống")]
    [JsonPropertyName("address")]
    public required string Address { get; set; }
    [JsonPropertyName("password")]
    public string Password { get; set; } = string.Empty;
    [JsonPropertyName("avatar")] 
public string Avatar { get; set; } = string.Empty;
}