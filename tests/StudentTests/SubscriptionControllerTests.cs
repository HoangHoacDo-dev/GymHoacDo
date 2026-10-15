using Xunit;
using Moq;
using Microsoft.AspNetCore.Mvc;
using API.Controllers;
using Core.Interfaces;
using Core.DTOs;
using System.Threading.Tasks;
using System;

namespace StudentTests;

public class SubscriptionControllerTests
{
    private readonly Mock<ISubscriptionService> _mockService;
    private readonly SubscriptionController _controller;

    public SubscriptionControllerTests()
    {
        _mockService = new Mock<ISubscriptionService>();
        _controller = new SubscriptionController(_mockService.Object);
    }

    [Fact]
    public async Task CreateSubscription_ValidRequest_Returns201Created()
    {
     
        var request = new CreateSubscriptionRequest { MemberId = 1, PackageId = 1, EmployeeId = 1 };
        var expectedResponse = new SubscriptionResponse 
        { 
            SubscriptionId = 10, 
            Status = "Active" 
        };

        _mockService.Setup(s => s.CreateSubscriptionAsync(request))
                    .ReturnsAsync(expectedResponse);

        var result = await _controller.CreateSubscription(request);

        var createdResult = Assert.IsType<CreatedResult>(result);
        Assert.Equal(201, createdResult.StatusCode);
    }

    [Fact]
    public async Task UnfreezeSubscription_Returns200Ok()
    {
     
        int subId = 1;
        _mockService.Setup(s => s.UnfreezeSubscriptionAsync(subId))
                    .ReturnsAsync("Hủy bảo lưu thành công!");

        var result = await _controller.UnfreezeSubscription(subId);

        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(200, okResult.StatusCode);
    }
}