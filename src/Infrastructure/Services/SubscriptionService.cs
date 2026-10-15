using Core.DTOs;
using Core.Entities;
using Core.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.Services;

public class SubscriptionService : ISubscriptionService
{
    private readonly AppDbContext _context;

    public SubscriptionService(AppDbContext context)
    {
        _context = context;
    }

public async Task<SubscriptionResponse> CreateSubscriptionAsync(CreateSubscriptionRequest request)
{
  
    var member = await _context.Members.FindAsync(request.MemberId);
    if (member == null) throw new Exception("Hội viên không tồn tại!");

    var package = await _context.Packages.FindAsync(request.PackageId);
    if (package == null) throw new Exception("Gói tập không tồn tại!");

    DateTime startDate = DateTime.Now.Date;

    var lastSubscription = await _context.Subscriptions
        .Where(s => s.MemberId == request.MemberId)
        .OrderByDescending(s => s.EndDate)
        .FirstOrDefaultAsync();

    if (lastSubscription != null && lastSubscription.EndDate > startDate)
    {
        startDate = lastSubscription.EndDate;
    }

    DateTime endDate = startDate.AddDays(package.DurationInDays);

    var subscription = new Subscription
    {
        MemberId = request.MemberId,
        PackageId = request.PackageId,
        EmployeeId = request.EmployeeId,
        StartDate = startDate,
        EndDate = endDate,
        Status = "Active"
    };

    member.Status = "Active";

    _context.Subscriptions.Add(subscription);

    await _context.SaveChangesAsync();

    return new SubscriptionResponse
    {
        SubscriptionId = subscription.Id,
        StartDate = subscription.StartDate.ToString("yyyy-MM-dd"),
        EndDate = subscription.EndDate.ToString("yyyy-MM-dd"),
        Status = subscription.Status
    };
}
    public async Task<string> FreezeSubscriptionAsync(int subscriptionId, FreezeRequest request)
    {
       
        if (request.StartDate >= request.EndDate)
            throw new Exception("Thời gian bảo lưu không hợp lệ (Ngày bắt đầu phải trước ngày kết thúc).");

        var subscription = await _context.Subscriptions.FindAsync(subscriptionId);
        if (subscription == null)
            throw new Exception("Không tìm thấy thẻ tập này!");

        if (subscription.EndDate < request.StartDate)
            throw new Exception("Thẻ tập đã hết hạn, không thể bảo lưu!");

     
        int freezeDays = (request.EndDate - request.StartDate).Days;

    
        subscription.EndDate = subscription.EndDate.AddDays(freezeDays);

        var freezeLog = new FreezeLog
        {
            SubscriptionId = subscriptionId,
            StartDate = request.StartDate,
            EndDate = request.EndDate,
            Reason = request.Reason
        };

        _context.FreezeLogs.Add(freezeLog);

        await _context.SaveChangesAsync();

        return $"Bảo lưu thành công. Thẻ sẽ được cộng thêm {freezeDays} ngày.";
    }
public async Task<string> UnfreezeSubscriptionAsync(int subscriptionId)
{
    var today = DateTime.Now.Date;
    var subscription = await _context.Subscriptions.FindAsync(subscriptionId);
    if (subscription == null) throw new Exception("Không tìm thấy thẻ tập!");

    var activeFreezes = await _context.FreezeLogs
        .Where(f => f.SubscriptionId == subscriptionId
                   && f.EndDate.Date > today)  
        .ToListAsync();

    if (!activeFreezes.Any())
        throw new Exception("Thẻ này không trong thời gian bảo lưu!");

    var firstFreeze = activeFreezes.First();

    int remainingFreezeDays = (firstFreeze.EndDate.Date - today).Days;

   
    subscription.EndDate = subscription.EndDate.AddDays(-remainingFreezeDays);

    foreach (var freeze in activeFreezes)
        freeze.EndDate = today;

    await _context.SaveChangesAsync();
    return $"Hủy bảo lưu thành công! Ngày hết hạn mới: {subscription.EndDate:yyyy-MM-dd}";
}
public async Task<CurrentSubscriptionDto?> GetCurrentSubscriptionAsync(int memberId)
{
    var today = DateTime.Now.Date;

    var sub = await _context.Subscriptions
        .Where(s => s.MemberId == memberId)
        .OrderByDescending(s => s.EndDate)
        .FirstOrDefaultAsync();

    if (sub == null) return null;

    var package = await _context.Packages.FindAsync(sub.PackageId);
    
    var isFrozen = await _context.FreezeLogs
        .AnyAsync(f => f.SubscriptionId == sub.Id 
                    && f.StartDate.Date <= today 
                    && f.EndDate.Date > today);  

    int remaining = (sub.EndDate.Date - today).Days;
    
    string status = isFrozen ? "Frozen (Đang bảo lưu)" : (remaining >= 0 ? "Active (Đang hoạt động)" : "Expired (Đã hết hạn)");

    return new CurrentSubscriptionDto
    {
        SubscriptionId = sub.Id,
        PackageName = package?.Name ?? "Không xác định",
        StartDate = sub.StartDate.ToString("yyyy-MM-dd"),
        EndDate = sub.EndDate.ToString("yyyy-MM-dd"),
        RemainingDays = remaining < 0 ? 0 : remaining,
        Status = status
    };
}
}