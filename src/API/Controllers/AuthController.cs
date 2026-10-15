using Core.DTOs;
using Core.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Email) || string.IsNullOrWhiteSpace(request.Password))
            {
                return BadRequest(new { status = "error", message = "Email và mật khẩu không được để trống." });
            }

            var response = await _authService.LoginAsync(request);

            if (response.Status == "error")
            {
                // Trả về 401 Unauthorized nếu sai tài khoản
                return Unauthorized(response); 
            }

            return Ok(response);
        }
    }
}