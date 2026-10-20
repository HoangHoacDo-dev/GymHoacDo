using Microsoft.AspNetCore.Mvc;
using Core.DTOs;
using Core.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CheckInController : ControllerBase
{
    private readonly ICheckInService _checkInService;
    private readonly AppDbContext _context;

    public CheckInController(ICheckInService checkInService, AppDbContext context)
    {
        _checkInService = checkInService;
        _context = context;
    }

    [HttpPost]
    public async Task<IActionResult> CheckIn([FromBody] CheckInRequest request)
    {
        try
        {
            var response = await _checkInService.ProcessCheckInAsync(request);
            return Ok(response);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { status = "error", message = ex.Message });
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new { status = "error", message = ex.Message });
        }
        catch (UnauthorizedAccessException ex)
        {
            return StatusCode(403, new { status = "error", message = ex.Message });
        }
        catch (Exception ex)
        {
            return BadRequest(new { status = "error", message = ex.Message });
        }
    }

    [HttpGet("History/{member_id}")]
    public async Task<IActionResult> GetHistory(int member_id)
    {
        try
        {
            var history = await _checkInService.GetCheckInHistoryAsync(member_id);
            return Ok(new { status = "success", total_checkins = history.Count(), data = history });
        }
        catch (Exception ex)
        {
            return BadRequest(new { status = "error", message = ex.Message });
        }
    }

    [HttpGet("Today")]
    public async Task<IActionResult> GetTodayHistory()
    {
        try
        {
            var today = DateTime.Now.Date;
            var logs = await _context.CheckInLogs
                .Where(l => l.CheckInTime.Date == today)
                .OrderByDescending(l => l.CheckInTime)
                .Take(15)
                .Select(l => new 
                {
                    member_id = l.MemberId,
                    member_name = _context.Members.Where(m => m.Id == l.MemberId).Select(m => m.FullName).FirstOrDefault(),
                    phone_number = _context.Members.Where(m => m.Id == l.MemberId).Select(m => m.PhoneNumber).FirstOrDefault(),
                    checkin_time = l.CheckInTime
                })
                .ToListAsync();

            return Ok(new { status = "success", data = logs });
        }
        catch (Exception ex)
        {
            return BadRequest(new { status = "error", message = ex.Message });
        }
    }
}
