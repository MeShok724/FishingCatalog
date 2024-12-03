using FishingCatalog.Core;
using FishingCatalog.msCatalog.Contracts;
using FishingCatalog.msCatalog.Controllers;
using FishingCatalog.msCatalog.Repositories;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace FishingCatalog.Tests
{
    public class ProductControllerTests
    {
        private readonly Mock<IProductRepository> _mockProductRepository;
        private readonly ProductController _controller;
        private readonly byte[] img = [];

        public ProductControllerTests()
        {
            _mockProductRepository = new Mock<IProductRepository>();
            _controller = new ProductController(_mockProductRepository.Object);
        }

        [Fact]
        public async Task GetProducts_ReturnsOkResult_WithProducts()
        {
            // Arrange
            var products = new List<Product>
            {
                Product.Create(Guid.NewGuid(), "Product1", 100, "Category1", "Description1", img).Item1,
                Product.Create(Guid.NewGuid(), "Product2", 200, "Category2", "Description2", img).Item1
            };
            _mockProductRepository.Setup(repo => repo.GetAll()).ReturnsAsync(products);

            // Act
            var result = await _controller.GetProducts();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnValue = Assert.IsType<List<ProductResponse>>(okResult.Value);
            Assert.Equal(2, returnValue.Count);
        }

        [Fact]
        public async Task GetById_ReturnsOkResult_WithProduct()
        {
            // Arrange
            var productId = Guid.NewGuid();
            var product = Product.Create(productId, "Product1", 100, "Category1", "Description1", img).Item1;
            _mockProductRepository.Setup(repo => repo.GetById(productId)).ReturnsAsync(product);

            // Act
            var result = await _controller.GetById(productId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnValue = Assert.IsType<ProductResponse>(okResult.Value);
            Assert.Equal(productId, returnValue.Id);
        }

        [Fact]
        public async Task AddProduct_ReturnsOkResult_WithProductId()
        {
            // Arrange
            var productRequest = new ProductRequest
            (
                "Product1",
                100,
                "Category1",
                "Description1",
                img
            );
            Guid newId = Guid.NewGuid();
            var newProduct = Product.Create(newId, productRequest.Name, productRequest.Price, productRequest.Category, productRequest.Description, productRequest.Image);
            _mockProductRepository
                .Setup(repo => repo.Add(It.IsAny<Product>()))
                .ReturnsAsync(newId);

            // Act
            var result = await _controller.AddProduct(productRequest);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            Assert.Equal(newProduct.Item1.Id, okResult.Value);
        }

        [Fact]
        public async Task Update_ReturnsOkResult_WithProductId()
        {
            // Arrange
            var productId = Guid.NewGuid();
            var productRequest = new ProductRequest
            (
                "UpdatedProduct",
                150,
                "UpdatedCategory",
                "UpdatedDescription",
                img
            );
            var creating = Product.Create(productId, productRequest.Name, productRequest.Price, productRequest.Category, productRequest.Description, productRequest.Image);
            var updatedProduct = creating.Item1;
            Assert.True(string.IsNullOrEmpty(creating.Item2), $"Item2 contains: {creating.Item2}");
            _mockProductRepository
                .Setup(repo => repo.Update(It.IsAny<Product>(), productId))
                .ReturnsAsync(productId);

            // Act
            var result = await _controller.Update(productId, productRequest);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            Assert.Equal(productId, okResult.Value);
        }

        [Fact]
        public async Task Delete_ReturnsOkResult_WithDeletedProductId()
        {
            // Arrange
            var productId = Guid.NewGuid();
            _mockProductRepository.Setup(repo => repo.Delete(productId)).ReturnsAsync(productId);

            // Act
            var result = await _controller.Delete(productId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            Assert.Equal(productId, okResult.Value);
        }

        [Fact]
        public async Task GetByCategory_ReturnsOkResult_WithProducts()
        {
            // Arrange
            var category = "Category1";
            var products = new List<Product>
            {
                Product.Create(Guid.NewGuid(), "Product1", 100, "Category1", "Description1", img).Item1,
                Product.Create(Guid.NewGuid(), "Product2", 200, "Category2", "Description2", img).Item1
            };
            _mockProductRepository.Setup(repo => repo.GetByCategory(category)).ReturnsAsync(products);

            // Act
            var result = await _controller.GetByCategory(category);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnValue = Assert.IsType<List<ProductResponse>>(okResult.Value);
            Assert.Equal(2, returnValue.Count);
        }

        [Fact]
        public async Task Sort_ReturnsOkResult_WithSortedProducts()
        {
            // Arrange
            var products = new List<Product>
            {
                Product.Create(Guid.NewGuid(), "Product1", 100, "Category1", "Description1", img).Item1,
                Product.Create(Guid.NewGuid(), "Product2", 200, "Category2", "Description2", img).Item1
            };
            _mockProductRepository.Setup(repo => repo.SortByPrice(true)).ReturnsAsync(products);

            // Act
            var result = await _controller.Sort("price", true);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnValue = Assert.IsType<List<ProductResponse>>(okResult.Value);
            Assert.Equal("Product1", returnValue[0].Name);
        }
    }
}
