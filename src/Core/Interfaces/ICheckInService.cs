using Core.DTOs;
using System.Threading.Tasks;

namespace Core.Interfaces;

public interface ICheckInService
{
    Task<CheckInResponse> ProcessCheckInAsync(CheckInRequest request);
    Task<IEnumerable<string>> GetCheckInHistoryAsync(int memberId);
}
