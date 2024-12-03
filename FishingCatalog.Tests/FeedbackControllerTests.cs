using FishingCatalog.Core;
using FishingCatalog.msFeedback.Contracts;
using FishingCatalog.msFeedback.Controllers;
using FishingCatalog.msFeedback.Repositories;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FishingCatalog.Tests
{
    public class FeedbackControllerTests
    {
        private readonly Mock<IFeedbackRepository> _feedbackRepositoryMock;
        private readonly FeedbackController _controller;

        public FeedbackControllerTests()
        {
            _feedbackRepositoryMock = new Mock<IFeedbackRepository>();
            _controller = new FeedbackController(_feedbackRepositoryMock.Object);
        }

        [Fact]
        public async Task Get_ShouldReturnAllFeedbacks()
        {
            // Arrange
            var feedbackList = new List<Feedback>
        {
            new Feedback(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), DateTime.UtcNow, "Test Comment 1"),
            new Feedback(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), DateTime.UtcNow, "Test Comment 2")
        };
            _feedbackRepositoryMock.Setup(repo => repo.GetAll()).ReturnsAsync(feedbackList);

            // Act
            var result = await _controller.Get();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnedFeedbacks = Assert.IsType<List<Feedback>>(okResult.Value);
            Assert.Equal(2, returnedFeedbacks.Count);
        }

        [Fact]
        public async Task GetById_ShouldReturnFeedback_WhenFound()
        {
            // Arrange
            var feedbackId = Guid.NewGuid();
            var feedback = new Feedback(feedbackId, Guid.NewGuid(), Guid.NewGuid(), DateTime.UtcNow, "Test Comment");
            _feedbackRepositoryMock.Setup(repo => repo.GetById(feedbackId)).ReturnsAsync(feedback);

            // Act
            var result = await _controller.GetById(feedbackId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnedFeedback = Assert.IsType<Feedback>(okResult.Value);
            Assert.Equal(feedbackId, returnedFeedback.Id);
        }

        [Fact]
        public async Task GetByUser_ShouldReturnFeedbacksByUser()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var feedbackList = new List<Feedback>
        {
            new Feedback(Guid.NewGuid(), userId, Guid.NewGuid(), DateTime.UtcNow, "Comment 1"),
            new Feedback(Guid.NewGuid(), userId, Guid.NewGuid(), DateTime.UtcNow, "Comment 2")
        };
            _feedbackRepositoryMock.Setup(repo => repo.GetByUserId(userId)).ReturnsAsync(feedbackList);

            // Act
            var result = await _controller.GetByUser(userId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnedFeedbacks = Assert.IsType<List<Feedback>>(okResult.Value);
            Assert.Equal(2, returnedFeedbacks.Count);
        }

        [Fact]
        public async Task Add_ShouldReturnNewFeedbackId_WhenSuccessful()
        {
            // Arrange
            var feedbackRequest = new FeedbackRequest
            (
                Guid.NewGuid(),
                Guid.NewGuid(),
                "Test Comment"
            );
            var newFeedbackId = Guid.NewGuid();
            _feedbackRepositoryMock.Setup(repo => repo.Add(It.IsAny<Feedback>())).ReturnsAsync(newFeedbackId);

            // Act
            var result = await _controller.Add(feedbackRequest);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            Assert.Equal(newFeedbackId, okResult.Value);
        }

        [Fact]
        public async Task Update_ShouldReturnUpdatedFeedbackId_WhenSuccessful()
        {
            // Arrange
            var feedbackId = Guid.NewGuid();
            var feedbackRequest = new FeedbackRequest
            (
                Guid.NewGuid(),
                Guid.NewGuid(),
                "Test Comment"
            );
            _feedbackRepositoryMock.Setup(repo => repo.Update(It.IsAny<Feedback>(), feedbackId)).ReturnsAsync(feedbackId);

            // Act
            var result = await _controller.Update(feedbackId, feedbackRequest);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            Assert.Equal(feedbackId, okResult.Value);
        }

        [Fact]
        public async Task Delete_ShouldReturnDeletedFeedbackId()
        {
            // Arrange
            var feedbackId = Guid.NewGuid();
            _feedbackRepositoryMock.Setup(repo => repo.Delete(feedbackId)).ReturnsAsync(feedbackId);

            // Act
            var result = await _controller.Delete(feedbackId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            Assert.Equal(feedbackId, okResult.Value);
        }
    }
}
