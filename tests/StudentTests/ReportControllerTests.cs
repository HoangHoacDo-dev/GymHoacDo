using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Xunit;
using Moq;
using Microsoft.AspNetCore.Mvc;
using API.Controllers;
using Core.Interfaces;
using Core.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;
namespace StudentTests;

public class ReportControllerTests
{
    private readonly Mock<IReportService> _mockService;
    private readonly ReportController _controller;

 public ReportControllerTests()
{
    _mockService = new Mock<IReportService>();
    
   var options = new DbContextOptionsBuilder<AppDbContext>()
    .UseInMemoryDatabase(databaseName: "TestDb_Report")
    .Options;

var mockContext = new AppDbContext(options);
    
    _controller = new ReportController(_mockService.Object, mockContext);
}

    [Fact]
    public async Task GetExpiringMembers_Returns200Ok()
    {
        int daysLeft = 7;
        var mockData = new List<ExpiringMemberDto> 
        { 
            new ExpiringMemberDto { MemberName = "Hoàng Hoắc Đô", RemainingDays = 3 } 
        };
        
        _mockService.Setup(s => s.GetExpiringMembersAsync(daysLeft))
                    .ReturnsAsync(mockData);

        var result = await _controller.GetExpiringMembers(daysLeft);

       
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(200, okResult.StatusCode);
    }
}