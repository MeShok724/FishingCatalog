using FishinfCatalog.msAuthorization.Controllers;
using FishingCatalog.Core;
using FishingCatalog.msUser.Contracts;
using FishingCatalog.msUser.Infrastructure;
using FishingCatalog.msUser.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FishingCatalog.Tests
{
    public class AuthenticationControllerTests
    {
        private readonly Mock<IUserRepository> _userRepositoryMock = new();
        private readonly Mock<IJwtProvider> _jwtProviderMock = new();
        private readonly AuthenticationController _controller;

        public AuthenticationControllerTests()
        {
            _controller = new AuthenticationController(_userRepositoryMock.Object, _jwtProviderMock.Object);

            // Добавление контекста HTTP для тестируемого контроллера
            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext()
            };
        }

        [Fact]
        public async Task Login_ShouldReturnNotFound_WhenUserDoesNotExist()
        {
            // Arrange
            _userRepositoryMock
                .Setup(repo => repo.GetByEmail(It.IsAny<string>()))
                .ReturnsAsync((User)null);

            var request = new AuthorizationRequest
            (
                "test@example.com",
                "password123"
            );

            // Act
            var result = await _controller.Login(request);

            // Assert
            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public async Task Login_ShouldReturnBadRequest_WhenPasswordIsInvalid()
        {
            // Arrange
            var creating = User.Create(Guid.NewGuid(), "user", "test@example.com", PasswordHasher.Generate("correctPassword"),
                DateTime.UtcNow, DateTime.UtcNow, true, Guid.NewGuid()
            );
            var user = creating.Item1;

            _userRepositoryMock
                .Setup(repo => repo.GetByEmail(It.IsAny<string>()))
                .ReturnsAsync(user);

            var request = new AuthorizationRequest
            (
                "test@example.com",
                "wrongPassword"
            );

            // Act
            var result = await _controller.Login(request);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
            Assert.Equal("Wrong password", badRequestResult.Value);
        }

        [Fact]
        public async Task Login_ShouldReturnOkWithToken_WhenLoginIsSuccessful()
        {
            // Arrange
            var creating = User.Create(Guid.NewGuid(), "user", "test@example.com", PasswordHasher.Generate("password123"),
                DateTime.UtcNow, DateTime.UtcNow, true, Guid.NewGuid()
            );
            var user = creating.Item1;

            var token = "jwt_token";
            _userRepositoryMock
                .Setup(repo => repo.GetByEmail(It.IsAny<string>()))
                .ReturnsAsync(user);

            _jwtProviderMock
                .Setup(jwt => jwt.GenerateToken(It.IsAny<User>()))
                .Returns(token);

            var request = new AuthorizationRequest
            (
                "test@example.com",
                "password123"
            );

            // Act
            var result = await _controller.Login(request);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            Assert.Equal(token, okResult.Value);
        }
    }
}
