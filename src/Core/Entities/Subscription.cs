using System;

namespace Core.Entities;

public class Subscription
{
    public int Id { get; set; }
    public int MemberId { get; set; }
    public int PackageId { get; set; }
    public int EmployeeId { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public string Status { get; set; } = "Active";
}