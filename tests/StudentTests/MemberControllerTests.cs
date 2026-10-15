using Xunit;
using Moq;
using Microsoft.AspNetCore.Mvc;
using API.Controllers; 
using Core.Interfaces;
using Core.DTOs;
namespace StudentTests
{
    public class MemberControllerTests
    {
        private readonly Mock<IMemberService> _mockService;
        private readonly MemberController _controller;

        public MemberControllerTests()
        {
            _mockService = new Mock<IMemberService>();
            _controller = new MemberController(_mockService.Object);
        }

        [Fact]
        public async Task AddMember_ValidData_Returns201Created()
        {
            var newMember = new CreateMemberRequest
            { 
                FullName = "cristiano ronaldo", 
                PhoneNumber = "0987654321",
                Dob = DateOnly.Parse("1995-10-15"),
                Gender = "Nam",
                Address = "Vinh, Nghệ An"
            };

            var result = await _controller.CreateMember(newMember);

            var createdResult = Assert.IsType<CreatedResult>(result);
            Assert.Equal(201, createdResult.StatusCode);
        }
    }
}