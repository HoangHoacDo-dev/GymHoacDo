using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Xunit;
using Moq;
using Microsoft.AspNetCore.Mvc;
using API.Controllers;
using Core.Interfaces;
using Core.DTOs;
using System;
using System.Threading.Tasks;

namespace StudentTests;

public class CheckInControllerTests
{
    private readonly Mock<ICheckInService> _mockService;
    private readonly CheckInController _controller;

    public CheckInControllerTests()
    {
       _mockService = new Mock<ICheckInService>();

    var options = new DbContextOptionsBuilder<AppDbContext>()
        .UseInMemoryDatabase("TestDb_CheckIn")
        .Options;

    var mockContext = new AppDbContext(options);

    _controller = new CheckInController(_mockService.Object, mockContext);
    }

    [Fact]
    public async Task CheckIn_ValidMember_Returns200Ok()
    {
        // 1. Chuẩn bị dữ liệu giả lập
        var request = new CheckInRequest { PhoneNumber = "0987654321" };
        var expectedResponse = new CheckInResponse
        {
            Status = "success",
            Message = "Xin chào Hoàng Hoắc Đô. Check-in thành công.",
            CheckinTime = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss") 
        };

        _mockService.Setup(s => s.ProcessCheckInAsync(request))
                    .ReturnsAsync(expectedResponse);

        var result = await _controller.CheckIn(request);

        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(200, okResult.StatusCode);
    }

    [Fact]
    public async Task CheckIn_FrozenMember_Returns403Forbidden()
    {
       
        var request = new CheckInRequest { PhoneNumber = "0123456789" };
        
      
        _mockService.Setup(s => s.ProcessCheckInAsync(request))
                    .ThrowsAsync(new UnauthorizedAccessException("Thẻ đang trong thời gian bảo lưu!"));

      
        var result = await _controller.CheckIn(request);

        var objectResult = Assert.IsType<ObjectResult>(result);
        Assert.Equal(403, objectResult.StatusCode);
    }
}