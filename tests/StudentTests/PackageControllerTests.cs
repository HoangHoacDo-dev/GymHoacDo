using Xunit;
using Moq;
using Microsoft.AspNetCore.Mvc;
using API.Controllers;
using Core.Interfaces;
using Core.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace StudentTests;

public class PackageControllerTests
{
    private readonly Mock<IPackageService> _mockService;
    private readonly PackageController _controller;

    public PackageControllerTests()
    {
        _mockService = new Mock<IPackageService>();
        _controller = new PackageController(_mockService.Object);
    }

    [Fact]
    public async Task GetPackages_Returns200Ok()
    {
      
        var mockData = new List<PackageDto> 
        { 
            new PackageDto { Id = 1, Name = "Gym 1 Tháng", Price = 500000 } 
        };
        _mockService.Setup(s => s.GetPackagesAsync()).ReturnsAsync(mockData);

    
        var result = await _controller.GetPackages();

      
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(200, okResult.StatusCode);
    }
}