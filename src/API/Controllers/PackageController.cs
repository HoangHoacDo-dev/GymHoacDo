using Microsoft.AspNetCore.Mvc;
using Core.DTOs;
using Core.Interfaces;
using System;
using System.Threading.Tasks;

namespace API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PackageController : ControllerBase
{
    private readonly IPackageService _packageService;

    public PackageController(IPackageService packageService)
    {
        _packageService = packageService;
    }

    [HttpGet]
    public async Task<IActionResult> GetPackages()
    {
        try
        {
            var packages = await _packageService.GetPackagesAsync();

            return Ok(new
            {
                status = "success",
                data = packages
            });
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = "Lỗi hệ thống: " + ex.Message });
        }
    }
    [HttpPost]
    public async Task<IActionResult> CreatePackage([FromBody] PackageRequest request)
    {
        try
        {
            var newId = await _packageService.AddPackageAsync(request);
            return Created($"/api/Package/{newId}", new { status = "success", message = "Thêm gói tập thành công", id = newId });
        }
        catch (Exception ex)
        {
            return BadRequest(new { status = "error", message = ex.Message });
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdatePackage(int id, [FromBody] PackageRequest request)
    {
        try
        {
            var success = await _packageService.UpdatePackageAsync(id, request);
            if (!success) return NotFound(new { status = "error", message = "Không tìm thấy gói tập để cập nhật." });

            return Ok(new { status = "success", message = "Cập nhật gói tập thành công!" });
        }
        catch (Exception ex)
        {
            return BadRequest(new { status = "error", message = ex.Message });
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeletePackage(int id)
    {
        try
        {
            var success = await _packageService.DeletePackageAsync(id);
            if (!success) return NotFound(new { status = "error", message = "Không tìm thấy gói tập để xóa." });

            return Ok(new { status = "success", message = "Đã xóa gói tập thành công!" });
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
