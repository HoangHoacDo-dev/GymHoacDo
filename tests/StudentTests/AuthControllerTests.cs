using Xunit;
using Moq;
using Microsoft.AspNetCore.Mvc;
using API.Controllers;
using Core.Interfaces;
using Core.DTOs;
using System.Threading.Tasks;

namespace StudentTests
{
    public class AuthControllerTests
    {
        private readonly Mock<IAuthService> _mockAuthService;
        private readonly AuthController _controller;

        public AuthControllerTests()
        {
            _mockAuthService = new Mock<IAuthService>();
            _controller = new AuthController(_mockAuthService.Object);
        }

        [Fact]
        public async Task Login_ValidCredentials_Returns200Ok()
        {
            var request = new LoginRequest { Email = "admin@gym.com", Password = "123" };
            var expectedResponse = new LoginResponse 
            { 
                Status = "success", 
                Data = new LoginData { Token = "day_la_token_gia", Role = "Admin", FullName = "Nguyễn Văn Admin" } 
            };

            _mockAuthService.Setup(s => s.LoginAsync(request)).ReturnsAsync(expectedResponse);

            // 2. Gọi hàm
            var result = await _controller.Login(request);

            // 3. Kiểm tra kết quả
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedData = Assert.IsType<LoginResponse>(okResult.Value);
            
            Assert.Equal(200, okResult.StatusCode);
            Assert.Equal("success", returnedData.Status);
        }

        [Fact]
        public async Task Login_InvalidCredentials_Returns401Unauthorized()
        {
         
            var request = new LoginRequest { Email = "hoacdo", Password = "sai_roi" };
            var expectedResponse = new LoginResponse { Status = "error" };

         
            _mockAuthService.Setup(s => s.LoginAsync(request)).ReturnsAsync(expectedResponse);

           
            var result = await _controller.Login(request);

          
            var unauthorizedResult = Assert.IsType<UnauthorizedObjectResult>(result);
            Assert.Equal(401, unauthorizedResult.StatusCode);
        }
    }
}