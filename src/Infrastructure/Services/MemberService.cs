using Core.Entities;
using Core.Interfaces;
using Core.DTOs;
using Infrastructure.Data; 
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using BCrypt.Net;

namespace Infrastructure.Services;

public class MemberService : IMemberService
{
    private readonly AppDbContext _context;

    public MemberService(AppDbContext context)
    {
        _context = context;
    }

 
public async Task<int> AddMemberAsync(CreateMemberRequest request)
    {
        var isPhoneExist = await _context.Members.AnyAsync(m => m.PhoneNumber == request.PhoneNumber);
        if (isPhoneExist) throw new Exception("Số điện thoại này đã được đăng ký trong hệ thống!");

        string rawPassword = string.IsNullOrWhiteSpace(request.Password) ? "123456" : request.Password;
        string hashedPassword = BCrypt.Net.BCrypt.HashPassword(rawPassword);

        var member = new Member
        {
            FullName = request.FullName,
            PhoneNumber = request.PhoneNumber,
            Dob = request.Dob,
            Gender = request.Gender,
            Address = request.Address,
            Password = hashedPassword, 
            Avatar = request.Avatar,
            Status = "New" 
        };

        _context.Members.Add(member);
        await _context.SaveChangesAsync();

        return member.Id;
    }

  
    public async Task<PaginatedResponse<MemberDto>> GetMembersAsync(int page, int limit, string search)
    {
        var query = _context.Members.AsQueryable();

        if (!string.IsNullOrWhiteSpace(search))
        {
            search = search.ToLower().Trim();
            query = query.Where(m =>
                m.FullName.ToLower().Contains(search) ||
                m.PhoneNumber.Contains(search));
        }

        var totalRecords = await query.CountAsync();
        var totalPages = (int)Math.Ceiling(totalRecords / (double)limit);
        if (totalPages == 0) totalPages = 1;

        var members = await query
            .OrderByDescending(m => m.Id)
            .Skip((page - 1) * limit)
            .Take(limit)
            .Select(m => new MemberDto
            {
                Id = m.Id,
                FullName = m.FullName,
                PhoneNumber = m.PhoneNumber,
                Gender = m.Gender,
                Status = m.Status,
                Avatar = m.Avatar
            })
            .ToListAsync();

        return new PaginatedResponse<MemberDto>
        {
            Status = "success",
            Data = members,
            Pagination = new PaginationMeta
            {
                CurrentPage = page,
                TotalPages = totalPages
            }
        };
    }


    public async Task<bool> UpdateMemberAsync(int id, CreateMemberRequest request)
    {
        var member = await _context.Members.FindAsync(id);
        if (member == null) return false;

        member.FullName = request.FullName;
        member.PhoneNumber = request.PhoneNumber;
        member.Dob = request.Dob;
        member.Gender = request.Gender;
        member.Address = request.Address;

        _context.Members.Update(member);
        await _context.SaveChangesAsync();
        
        return true;
    }

  
    public async Task<bool> DeleteMemberAsync(int id)
    {
        var member = await _context.Members.FindAsync(id);
        if (member == null) return false;

        var hasSubscriptions = await _context.Subscriptions.AnyAsync(s => s.MemberId == id);
        if (hasSubscriptions)
        {
            throw new InvalidOperationException("Không thể xóa hội viên đã có lịch sử mua gói tập. Vui lòng kiểm tra lại!");
        }

        _context.Members.Remove(member);
        await _context.SaveChangesAsync();
        
        return true;
    }
}