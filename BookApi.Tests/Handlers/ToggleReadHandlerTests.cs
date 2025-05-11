using System.Threading;
using System.Threading.Tasks;
using BookApi.Application.Features.Books.Commands;
using BookApi.Application.Interfaces;
using Moq;
using Xunit;

namespace BookApi.Tests.Handlers
{
    public class ToggleReadHandlerTests
    {
        private readonly Mock<IBookRepository> _mockRepo;

        public ToggleReadHandlerTests()
        {
            _mockRepo = new Mock<IBookRepository>();
        }

        [Fact]
        public async Task Handle_Should_CallToggleReadAsync_AndReturnTrue()
        {
            // Arrange
            var handler = new ToggleReadHandler(_mockRepo.Object);
            var command = new ToggleReadCommand { Id = 1, UserId = 123 };

            _mockRepo
                .Setup(r => r.ToggleReadAsync(command.Id, command.UserId))
                .ReturnsAsync(true);

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(result);
            _mockRepo.Verify(r => r.ToggleReadAsync(command.Id, command.UserId), Times.Once);
        }
    }
}
