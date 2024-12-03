using FishingCatalog.Core;
using FishingCatalog.msUser.Controllers;
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
    public class RoleControllerTests
    {
        private readonly Mock<IRoleRepository> _roleRepositoryMock;
        private readonly RoleController _controller;

        public RoleControllerTests()
        {
            _roleRepositoryMock = new Mock<IRoleRepository>();
            _controller = new RoleController(_roleRepositoryMock.Object);
        }

        [Fact]
        public async Task GetRoles_ShouldReturnAllRoles()
        {
            // Arrange
            var roles = new List<Role>
            {
                Role.Create(Guid.NewGuid(), "Admin").Item1,
                Role.Create(Guid.NewGuid(), "User").Item1 
            };
            _roleRepositoryMock.Setup(repo => repo.GetAll()).ReturnsAsync(roles);

            // Act
            var result = await _controller.GetRoles();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            Assert.Equal(roles, okResult.Value);
        }

        [Fact]
        public async Task GetByName_ShouldReturnRole_WhenRoleExists()
        {
            // Arrange
            var roleName = "Admin";
            var role = Role.Create(Guid.NewGuid(), roleName).Item1;
            _roleRepositoryMock.Setup(repo => repo.GetByName(roleName)).ReturnsAsync(role.Id);

            // Act
            var result = await _controller.GetByName(roleName);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            Assert.Equal(role.Id, okResult.Value);
        }

        [Fact]
        public async Task GetByName_ShouldReturnOkWithNull_WhenRoleDoesNotExist()
        {
            // Arrange
            var roleName = "NonExistentRole";
            _roleRepositoryMock.Setup(repo => repo.GetByName(roleName)).ReturnsAsync(Guid.Empty);

            // Act
            var result = await _controller.GetByName(roleName);

            // Assert
            var errorResult = Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public async Task Add_ShouldReturnOk_WhenRoleIsValid()
        {
            // Arrange
            var roleName = "Manager";
            _roleRepositoryMock.Setup(repo => repo.Add(It.IsAny<Role>())).ReturnsAsync(Guid.NewGuid());

            // Act
            var result = await _controller.Add(roleName);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
        }

        [Fact]
        public async Task Add_ShouldReturnBadRequest_WhenRoleIsInvalid()
        {
            // Arrange
            var invalidRoleName = "";
            var invalidRole = Role.Create(Guid.NewGuid(), invalidRoleName);

            // Act
            var result = await _controller.Add(invalidRoleName);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
            Assert.Equal(invalidRole.Item2, badRequestResult.Value);
        }

        [Fact]
        public async Task Delete_ShouldReturnOk_WhenRoleIsDeleted()
        {
            // Arrange
            var roleId = Guid.NewGuid();
            _roleRepositoryMock.Setup(repo => repo.Delete(roleId)).ReturnsAsync(roleId);

            // Act
            var result = await _controller.Delete(roleId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            Assert.Equal(roleId, okResult.Value);
        }
    }
}
