using FishingCatalog.msNotification.Controllers;
using FishingCatalog.msNotification.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FishingCatalog.Tests
{
    public class NotificationControllerTests
    {
        private readonly Mock<IEmailService> _emailServiceMock;
        private readonly NotificationController _controller;

        public NotificationControllerTests()
        {
            _emailServiceMock = new Mock<IEmailService>();
            _controller = new NotificationController(_emailServiceMock.Object);
        }

        [Fact]
        public async Task SendToEmailList_ShouldReturnBadRequest_WhenListIsNull()
        {
            // Arrange
            List<string> emails = null;

            // Act
            var result = await _controller.SendToEmailList(emails, "Subject", "Text");

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Список email не может быть пустым", badRequestResult.Value);
        }

        [Fact]
        public async Task SendToEmailList_ShouldReturnBadRequest_WhenListIsEmpty()
        {
            // Arrange
            var emails = new List<string>();

            // Act
            var result = await _controller.SendToEmailList(emails, "Subject", "Text");

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Список email не может быть пустым", badRequestResult.Value);
        }

        [Fact]
        public async Task SendToEmailList_ShouldSendEmails_WhenListIsValid()
        {
            // Arrange
            var emails = new List<string> { "test1@example.com", "test2@example.com" };
            var subject = "Subject";
            var text = "Text";

            // Act
            var result = await _controller.SendToEmailList(emails, subject, text);

            // Assert
            _emailServiceMock.Verify(service => service.SendEmailAsync("test1@example.com", subject, text), Times.Once);
            _emailServiceMock.Verify(service => service.SendEmailAsync("test2@example.com", subject, text), Times.Once);

            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal("Письма успешно разосланы", okResult.Value);
        }
    }
}
