using Microsoft.AspNetCore.Mvc;
using Core.Interfaces;
using System;
using System.Threading.Tasks;
using Infrastructure.Data; 
using Microsoft.EntityFrameworkCore;

namespace API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ReportController : ControllerBase
{
    private readonly IReportService _reportService;
    private readonly AppDbContext _context;

    public ReportController(IReportService reportService, AppDbContext context)
    {
        _reportService = reportService;
        _context = context;
    }

    [HttpGet("Expiring")]
    public async Task<IActionResult> GetExpiringMembers([FromQuery] int days_left = 7)
    {
        try
        {
            var data = await _reportService.GetExpiringMembersAsync(days_left);
            return Ok(new
            {
                status = "success",
                data = data
            });
        }
        catch (Exception ex)
        {
            return BadRequest(new { status = "error", message = ex.Message });
        }
    }

    [HttpGet("Frozen")]
    public async Task<IActionResult> GetFrozenSubscriptions()
    {
        try
        {
            var data = await _reportService.GetFrozenSubscriptionsAsync();
            return Ok(new
            {
                status = "success",
                data = data
            });
        }
        catch (Exception ex)
        {
            return BadRequest(new { status = "error", message = ex.Message });
        }
    }

    [HttpGet("summary")]
    public async Task<IActionResult> GetSummary()
    {
        try 
        {
            
            var totalMembers = await _context.Members.CountAsync();
            var activeMembers = await _context.Members.CountAsync(m => m.Status != "New");
            
            var classesToday = 12;
            var monthlyRevenue = 75000000; 

            return Ok(new 
            { 
                status = "success", 
                data = new 
                {
                    totalMembers = totalMembers,
                    activeMembers = activeMembers,
                    classesToday = classesToday,
                    monthlyRevenue = monthlyRevenue
                }
            });
        }
        catch (Exception ex)
        {
            return BadRequest(new { status = "error", message = "Lỗi load tổng quan: " + ex.Message });
        }
    }
}