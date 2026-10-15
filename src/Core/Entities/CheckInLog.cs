using System;

namespace Core.Entities;

public class CheckInLog
{
    public int Id { get; set; }
    public int MemberId { get; set; }
    public DateTime CheckInTime { get; set; }
}

