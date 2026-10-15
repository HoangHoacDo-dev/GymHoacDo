using Core.DTOs;
using System.Threading.Tasks;

namespace Core.Interfaces;

public interface ISubscriptionService
{
    Task<SubscriptionResponse> CreateSubscriptionAsync(CreateSubscriptionRequest request);
    Task<string> FreezeSubscriptionAsync(int subscriptionId, FreezeRequest request);
    Task<string> UnfreezeSubscriptionAsync(int subscriptionId);
    Task<CurrentSubscriptionDto?> GetCurrentSubscriptionAsync(int memberId);
}