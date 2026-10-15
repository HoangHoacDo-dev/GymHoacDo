using Core.DTOs;
using Core.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using BCrypt.Net;

namespace Infrastructure.Services
{
    public class AuthService : IAuthService
    {
        private readonly IConfiguration _configuration;
        private readonly AppDbContext _context;

        public AuthService(IConfiguration configuration, AppDbContext context)
        {
            _configuration = configuration;
            _context = context;
        }

public async Task<LoginResponse> LoginAsync(LoginRequest request)
{

    var employee = await _context.Employees
        .FirstOrDefaultAsync(e => e.AccountName == request.Email);

    if (employee != null && BCrypt.Net.BCrypt.Verify(request.Password, employee.Password))
    {
        var token = GenerateJwtToken(employee.Id, employee.FullName, employee.Role, employee.AccountName);
        
        return new LoginResponse 
        { 
            Status = "success", 
            Message = $"Dang nhap {employee.Role} thanh cong",
            Data = new LoginData 
            { 
                Token = token,
                Role = employee.Role, 
                EmployeeId = employee.Id, 
                MemberId = 0, 
                FullName = employee.FullName 
            }
        };
    }

    var member = await _context.Members
        .FirstOrDefaultAsync(m => m.PhoneNumber == request.Email);

    if (member != null && BCrypt.Net.BCrypt.Verify(request.Password, member.Password))
    {
        var token = GenerateJwtToken(member.Id, member.FullName, "Member", member.PhoneNumber);
        
        return new LoginResponse 
        { 
            Status = "success", 
            Message = "Dang nhap Hoi vien thanh cong",
            Data = new LoginData 
            { 
                Token = token,
                Role = "Member", 
                EmployeeId = 0, 
                MemberId = member.Id, 
                FullName = member.FullName 
            }
        };
    }

    return new LoginResponse 
    { 
        Status = "error", 
        Message = "Sai tai khoan hoac mat khau!" 
    };
}

        private string GenerateJwtToken(int id, string name, string role, string email)
        {
            var jwtSettings = _configuration.GetSection("JwtSettings");
            var secretKey = jwtSettings["Secret"] ?? "DayLaMotChuoiBiMatSieuDaiVaPhucTapDungDeKyToken_Gym2026";
            
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, id.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, email),
                new Claim("FullName", name),
                new Claim(ClaimTypes.Role, role),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddHours(2),
                Issuer = jwtSettings["Issuer"] ?? "GymAPI",
                Audience = jwtSettings["Audience"] ?? "GymClients",
                SigningCredentials = creds
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }
    }
}