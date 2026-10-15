using Core.DTOs;

namespace Core.Interfaces;

public interface IMemberService
{
    Task<PaginatedResponse<MemberDto>> GetMembersAsync(int page, int limit, string search);
    Task<int> AddMemberAsync(CreateMemberRequest request);
    Task<bool> UpdateMemberAsync(int id, CreateMemberRequest request);
    Task<bool> DeleteMemberAsync(int id);
}