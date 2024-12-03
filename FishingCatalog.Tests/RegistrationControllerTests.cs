using FishingCatalog.Core;
using FishingCatalog.msUser.Contracts;
using FishingCatalog.msUser.Controllers;
using FishingCatalog.msUser.Infrastructure;
using FishingCatalog.msUser.Repositories;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FishingCatalog.Tests
{
    public class RegistrationControllerTests
    {
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly Mock<IRabbitMQService> _rabbitMQServiceMock;
        private readonly RegistrationController _controller;

        public RegistrationControllerTests()
        {
            _userRepositoryMock = new Mock<IUserRepository>();
            _rabbitMQServiceMock = new Mock<IRabbitMQService>();
            _controller = new RegistrationController(_userRepositoryMock.Object, _rabbitMQServiceMock.Object);
        }

        [Fact]
        public async Task Registrate_ShouldReturnBadRequest_WhenDefaultRoleIdNotFound()
        {
            // Arrange
            _userRepositoryMock.Setup(repo => repo.GetDafaultRoleId()).ReturnsAsync(Guid.Empty);
            var userRequest = new RegistrationRequest
            (
                "Test User",
                "test@example.com",
                "Password123"
            );

            // Act
            var result = await _controller.Registrate(userRequest);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
            Assert.Equal("Default role not found", badRequestResult.Value);
        }

        [Theory]
        [InlineData("short")]
        [InlineData("thispasswordiswaytoolongforourrequirements")]
        public async Task Registrate_ShouldReturnBadRequest_WhenPasswordIsInvalid(string invalidPassword)
        {
            // Arrange
            _userRepositoryMock.Setup(repo => repo.GetDafaultRoleId()).ReturnsAsync(Guid.NewGuid());
            var userRequest = new RegistrationRequest
            (
                "Test User",
                "test@example.com",
                invalidPassword
            );

            // Act
            var result = await _controller.Registrate(userRequest);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
            Assert.Equal("The password must be longer than 8 and shorter than 16 characters", badRequestResult.Value);
        }
    }
}
