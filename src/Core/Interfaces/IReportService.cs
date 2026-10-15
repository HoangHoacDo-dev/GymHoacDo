using Core.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Core.Interfaces;

public interface IReportService
{
    Task<IEnumerable<ExpiringMemberDto>> GetExpiringMembersAsync(int daysLeft);
    Task<object> GetFrozenSubscriptionsAsync();
}