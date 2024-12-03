using FishingCatalog.Core;
using FishingCatalog.msCart.Contracts;
using FishingCatalog.msCart.Controllers;
using FishingCatalog.msCart.Repositories;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace FishingCatalog.Tests
{
    public class CartControllerTests
    {
        private readonly Mock<ICartRepository> _mockCartRepository;
        private readonly CartController _controller;

        public CartControllerTests()
        {
            _mockCartRepository = new Mock<ICartRepository>();
            _controller = new CartController(_mockCartRepository.Object);
        }

        [Fact]
        public async Task Get_ReturnsOkResult_WithListOfCarts()
        {
            // Arrange
            var cartList = new List<Cart>
            {
                new(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), 1, DateTime.UtcNow, DateTime.UtcNow),
                new(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), 2, DateTime.UtcNow, DateTime.UtcNow)
            };
            _mockCartRepository.Setup(repo => repo.GetAll()).ReturnsAsync(cartList);

            // Act
            var result = await _controller.Get();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnValue = Assert.IsType<List<Cart>>(okResult.Value);
            Assert.Equal(cartList.Count, returnValue.Count);
        }

        [Fact]
        public async Task GetById_ReturnsOkResult_WithCart()
        {
            // Arrange
            var cartId = Guid.NewGuid();
            var cart = new Cart(cartId, Guid.NewGuid(), Guid.NewGuid(), 1, DateTime.UtcNow, DateTime.UtcNow);
            _mockCartRepository.Setup(repo => repo.GetById(cartId)).ReturnsAsync(cart);

            // Act
            var result = await _controller.GetById(cartId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnValue = Assert.IsType<Cart>(okResult.Value);
            Assert.Equal(cartId, returnValue.Id);
        }

        [Fact]
        public async Task GetByUser_ReturnsOkResult_WithListOfCarts()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var cartList = new List<Cart>
            {
                new(Guid.NewGuid(), userId, Guid.NewGuid(), 1, DateTime.UtcNow, DateTime.UtcNow),
                new(Guid.NewGuid(), userId, Guid.NewGuid(), 2, DateTime.UtcNow, DateTime.UtcNow)
            };
            _mockCartRepository.Setup(repo => repo.GetByUserId(userId)).ReturnsAsync(cartList);

            // Act
            var result = await _controller.GetByUser(userId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnValue = Assert.IsType<List<Cart>>(okResult.Value);
            Assert.Equal(cartList.Count, returnValue.Count);
        }

        [Fact]
        public async Task Add_ReturnsOkResult_WithCartId()
        {
            // Arrange
            var cartRequest = new CartRequest
            (
                Guid.NewGuid(),
                Guid.NewGuid(),
                1
            );
            var cartId = Guid.NewGuid();
            _mockCartRepository.Setup(repo => repo.Add(It.IsAny<Cart>())).ReturnsAsync(cartId);

            // Act
            var result = await _controller.Add(cartRequest);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnValue = Assert.IsType<Guid>(okResult.Value);
            Assert.Equal(cartId, returnValue);
        }

        [Fact]
        public async Task Update_ReturnsOkResult_WithUpdatedCartId()
        {
            // Arrange
            var cartId = Guid.NewGuid();
            var cartRequest = new CartRequest
            (
                Guid.NewGuid(),
                Guid.NewGuid(),
                1
            );
            _mockCartRepository.Setup(repo => repo.Update(It.IsAny<Cart>(), cartId)).ReturnsAsync(cartId);

            // Act
            var result = await _controller.Update(cartId, cartRequest);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnValue = Assert.IsType<Guid>(okResult.Value);
            Assert.Equal(cartId, returnValue);
        }

        [Fact]
        public async Task Delete_ReturnsOkResult_WithCartId()
        {
            // Arrange
            var cartId = Guid.NewGuid();
            _mockCartRepository.Setup(repo => repo.Delete(cartId)).ReturnsAsync(cartId);

            // Act
            var result = await _controller.Delete(cartId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnValue = Assert.IsType<Guid>(okResult.Value);
            Assert.Equal(cartId, returnValue);
        }

        [Fact]
        public async Task DeleteByUser_ReturnsOkResult_WithUserId()
        {
            // Arrange
            var userId = Guid.NewGuid();
            _mockCartRepository.Setup(repo => repo.DeleteAllByUserId(userId)).ReturnsAsync(userId);

            // Act
            var result = await _controller.DeleteByUser(userId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnValue = Assert.IsType<Guid>(okResult.Value);
            Assert.Equal(userId, returnValue);
        }
    }
}