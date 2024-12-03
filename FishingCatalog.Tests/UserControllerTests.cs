using FishingCatalog.Core;
using FishingCatalog.msUser.Contracts;
using FishingCatalog.msUser.Controllers;
using FishingCatalog.msUser.Infrastructure;
using FishingCatalog.msUser.Repositories;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Xml.Linq;
using Xunit;

namespace FishingCatalog.Tests
{
    public class UserControllerTests
    {
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly Mock<IRabbitMQService> _rabbitMQServiceMock;
        private readonly UserController _controller;

        public UserControllerTests()
        {
            _userRepositoryMock = new Mock<IUserRepository>();
            _rabbitMQServiceMock = new Mock<IRabbitMQService>();
            _controller = new UserController(_userRepositoryMock.Object, _rabbitMQServiceMock.Object);
        }

        [Fact]
        public async Task Get_ShouldReturnAllUsers()
        {
            // Arrange
            var users = new List<User>
            {
                User.Create(Guid.NewGuid(), "John", "john@example.com", "passwordhash", DateTime.UtcNow, DateTime.UtcNow, false, Guid.NewGuid()).Item1,
                User.Create(Guid.NewGuid(), "Jasdd", "jodfhn@example.com", "passwordhash", DateTime.UtcNow, DateTime.UtcNow, false, Guid.NewGuid()).Item1
            };
            _userRepositoryMock.Setup(repo => repo.GetAll()).ReturnsAsync(users);

            // Act
            var result = await _controller.Get();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnedUsers = Assert.IsType<List<UserResponse>>(okResult.Value);
            Assert.Equal(2, returnedUsers.Count);
        }

        [Fact]
        public async Task GetById_ShouldReturnUser_WhenUserExists()
        {
            // Arrange
            var user = User.Create(Guid.NewGuid(), "John", "john@example.com", "passwordhash", DateTime.UtcNow, DateTime.UtcNow, false, Guid.NewGuid()).Item1;
            _userRepositoryMock.Setup(repo => repo.GetById(user.Id)).ReturnsAsync(user);

            // Act
            var result = await _controller.GetById(user.Id);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnedUser = Assert.IsType<UserResponse>(okResult.Value);
            Assert.Equal(user.Id, returnedUser.Id);
        }

        [Fact]
        public async Task GetById_ShouldReturnBadRequest_WhenUserDoesNotExist()
        {
            // Arrange
            var userId = Guid.NewGuid();
            _userRepositoryMock.Setup(repo => repo.GetById(userId)).ReturnsAsync((User)null);

            // Act
            var result = await _controller.GetById(userId);

            // Assert
            Assert.IsType<BadRequestResult>(result.Result);
        }

        [Fact]
        public async Task Delete_ShouldSendGuidToRabbitMQ()
        {
            // Arrange
            var userId = Guid.NewGuid();
            _userRepositoryMock.Setup(repo => repo.Delete(userId)).ReturnsAsync(userId);

            await _rabbitMQServiceMock.Object.InitializeAsync();

            // Act
            var result = await _controller.Delete(userId);

            // Assert
            _rabbitMQServiceMock.Verify(service => service.SendGuidAsync(userId), Times.Once);
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            Assert.Equal(userId, okResult.Value);
        }

        [Fact]
        public async Task Update_ShouldReturnUpdatedUserId()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var userToUpdate = new UserToUpdate(
                "string Name",
                "string Email",
                "string PasswordHash",
                DateTime.UtcNow,
                DateTime.UtcNow,
                true,
                Guid.NewGuid()
            );
            _userRepositoryMock.Setup(repo => repo.Update(It.IsAny<User>(), userId)).ReturnsAsync(userId);

            // Act
            var result = await _controller.Update(userId, userToUpdate);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            Assert.Equal(userId, okResult.Value);
        }
    }
}
