using Core.DTOs;
using Core.Entities;
using Core.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.Services;

public class CheckInService : ICheckInService
{
    private readonly AppDbContext _context;

    public CheckInService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<CheckInResponse> ProcessCheckInAsync(CheckInRequest request)
    {
        var member = await _context.Members
            .FirstOrDefaultAsync(m => m.PhoneNumber == request.PhoneNumber);

        if (member == null)
            throw new Exception("Không tìm thấy hội viên với số điện thoại này!");

        
        var subscription = await _context.Subscriptions
            .Where(s => s.MemberId == member.Id)
            .OrderByDescending(s => s.EndDate)
            .FirstOrDefaultAsync();

        
        if (subscription == null)
            throw new UnauthorizedAccessException("Hội viên chưa mua gói tập nào!");

        DateTime today = DateTime.Now.Date;

    
        if (subscription.EndDate.Date < today)
            throw new UnauthorizedAccessException("Thẻ đã hết hạn!");

       
        var isFrozen = await _context.FreezeLogs
            .AnyAsync(f => f.SubscriptionId == subscription.Id
                        && today >= f.StartDate.Date
                        && today < f.EndDate.Date);

        if (isFrozen)
            throw new UnauthorizedAccessException("Thẻ đang trong thời gian bảo lưu (Frozen)!");

        var checkInLog = new CheckInLog
        {
            MemberId = member.Id,
            CheckInTime = DateTime.Now
        };

        _context.CheckInLogs.Add(checkInLog);
        await _context.SaveChangesAsync();

        return new CheckInResponse
        {
            Status = "success",
            Message = $"Xin chào {member.FullName}. Check-in thành công.",
            CheckinTime = checkInLog.CheckInTime.ToString("yyyy-MM-ddTHH:mm:ss")
        };
    }
    public async Task<IEnumerable<string>> GetCheckInHistoryAsync(int memberId)
    {
        
        var history = await _context.CheckInLogs
            .Where(c => c.MemberId == memberId)
            .OrderByDescending(c => c.CheckInTime)
            .Select(c => c.CheckInTime.ToString("yyyy-MM-dd HH:mm:ss"))
            .ToListAsync();

        return history;
    }
}