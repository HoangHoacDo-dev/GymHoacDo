using Core.DTOs;
using Core.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.Services;

public class ReportService : IReportService
{
    private readonly AppDbContext _context;

    public ReportService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<ExpiringMemberDto>> GetExpiringMembersAsync(int daysLeft)
    {
        var today = DateTime.Now.Date;
        var targetDate = today.AddDays(daysLeft);

        var expiringSubscriptions = await _context.Subscriptions
            .Join(_context.Members, s => s.MemberId, m => m.Id, (s, m) => new { s, m })
            .Join(_context.Packages, sm => sm.s.PackageId, p => p.Id, (sm, p) => new { sm.s, sm.m, p })
            .Where(x => x.s.EndDate.Date >= today && x.s.EndDate.Date <= targetDate)
            .OrderBy(x => x.s.EndDate)
            .Select(x => new ExpiringMemberDto
            {
                MemberName = x.m.FullName,
                PhoneNumber = x.m.PhoneNumber,
                PackageName = x.p.Name,
                EndDate = x.s.EndDate.ToString("yyyy-MM-dd"),
                RemainingDays = (x.s.EndDate.Date - today).Days
            })
            .ToListAsync();

        return expiringSubscriptions;
    }
public async Task<object> GetFrozenSubscriptionsAsync()
{
    var today = DateTime.Now.Date;

    var frozenLogs = await _context.FreezeLogs
        .Where(f => f.StartDate.Date <= today && f.EndDate.Date > today) 
        .ToListAsync();

    var result = frozenLogs.Select(f => {
        var sub = _context.Subscriptions.Find(f.SubscriptionId);
        var memberName = sub != null ? _context.Members.Find(sub.MemberId)?.FullName : "Không rõ";
        
        return new {
            freeze_id = f.Id,
            subscription_id = f.SubscriptionId,
            member_name = memberName,
            start_date = f.StartDate,
            end_date = f.EndDate,
            total_days = (f.EndDate.Date - f.StartDate.Date).Days,
            remaining_days = (f.EndDate.Date - today).Days
        };
    }).ToList();

    return result;
}
}