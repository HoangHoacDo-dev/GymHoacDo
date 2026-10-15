using Microsoft.AspNetCore.Mvc;
using Core.DTOs;
using Core.Interfaces;
using System;
using System.Threading.Tasks;

namespace API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class SubscriptionController : ControllerBase
{
    private readonly ISubscriptionService _subscriptionService;

    public SubscriptionController(ISubscriptionService subscriptionService)
    {
        _subscriptionService = subscriptionService;
    }

    [HttpPost]
    public async Task<IActionResult> CreateSubscription([FromBody] CreateSubscriptionRequest request)
    {
        try
        {
            var data = await _subscriptionService.CreateSubscriptionAsync(request);

            return Created("", new
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
    [HttpPost("{id}/freeze")]
    public async Task<IActionResult> FreezeSubscription(int id, [FromBody] FreezeRequest request)
    {
        try
        {
            var message = await _subscriptionService.FreezeSubscriptionAsync(id, request);

            return Ok(new
            {
                status = "success",
                message = message
            });
        }
        catch (Exception ex)
        {
            return BadRequest(new { status = "error", message = ex.Message });
        }
    }
    [HttpPost("{id}/unfreeze")]
    public async Task<IActionResult> UnfreezeSubscription(int id)
    {
        try
        {
            var message = await _subscriptionService.UnfreezeSubscriptionAsync(id);
            return Ok(new { status = "success", message = message });
        }
        catch (Exception ex)
        {
            return BadRequest(new { status = "error", message = ex.Message });
        }
    }
    [HttpGet("Member/{member_id}")]
    public async Task<IActionResult> GetCurrentSubscription(int member_id)
    {
        try
        {
            var data = await _subscriptionService.GetCurrentSubscriptionAsync(member_id);
            if (data == null) 
                return Ok(new { status = "success", message = "Hội viên này chưa đăng ký gói tập nào." });

            return Ok(new { status = "success", data = data });
        }
        catch (Exception ex)
        {
            return BadRequest(new { status = "error", message = ex.Message });
        }
    }
}