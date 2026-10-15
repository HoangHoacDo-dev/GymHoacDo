using Microsoft.AspNetCore.Mvc;
using Core.DTOs;
using Core.Interfaces;
using System;
using System.Threading.Tasks;

namespace API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class MemberController : ControllerBase
{
    private readonly IMemberService _memberService;

    public MemberController(IMemberService memberService)
    {
        _memberService = memberService;
    }


    [HttpGet]
    public async Task<IActionResult> GetMembers([FromQuery] int page = 1, [FromQuery] int limit = 10, [FromQuery] string search = "")
    {
        try
        {
            
            var response = await _memberService.GetMembersAsync(page, limit, search);
            
            
            return Ok(response);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = "Lỗi hệ thống: " + ex.Message });
        }
    }


    [HttpPost]
    public async Task<IActionResult> CreateMember([FromBody] CreateMemberRequest request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            var newMemberId = await _memberService.AddMemberAsync(request);

            return Created($"/api/Member/{newMemberId}", new
            {
                Message = "Thêm hội viên thành công!",
                id = newMemberId
            });
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = "Lỗi hệ thống: " + ex.Message });
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateMember(int id, [FromBody] CreateMemberRequest request)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        try
        {
            var success = await _memberService.UpdateMemberAsync(id, request);
            
            if (!success) 
                return NotFound(new { status = "error", message = "Không tìm thấy hội viên để cập nhật." });
            
            return Ok(new { status = "success", message = "Cập nhật thông tin hội viên thành công!" });
        }
        catch (Exception ex)
        {
            return BadRequest(new { status = "error", message = "Lỗi hệ thống: " + ex.Message });
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteMember(int id)
    {
        try
        {
            var success = await _memberService.DeleteMemberAsync(id);
            
            if (!success) 
                return NotFound(new { status = "error", message = "Không tìm thấy hội viên để xóa." });

            return Ok(new { status = "success", message = "Đã xóa hội viên thành công!" });
        }
        catch (InvalidOperationException ex)
        {
            
            return BadRequest(new { status = "error", message = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { status = "error", message = "Lỗi hệ thống: " + ex.Message });
        }
    }
}
