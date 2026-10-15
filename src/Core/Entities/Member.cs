namespace Core.Entities;

public class Member
{
    public string Status { get; set; } = "New";
    public int Id { get; set; }
    public required string FullName { get; set; }
    public required string PhoneNumber { get; set; }
    public required DateOnly Dob { get; set; }
    public required string Gender { get; set; }
    public required string Address { get; set; }
    public string Password { get; set; } = "123456";
    public string Avatar { get; set; } = string.Empty;
}