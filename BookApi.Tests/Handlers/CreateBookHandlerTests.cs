using System.Threading;
using System.Threading.Tasks;
using BookApi.Application.Features.Books.Commands;
using BookApi.Application.Interfaces;
using BookApi.Domain.Entities;
using Moq;
using Xunit;

namespace BookApi.Tests.Handlers
{
    public class CreateBookHandlerTests
    {
        private readonly Mock<IBookRepository> _mockRepo;

        public CreateBookHandlerTests()
        {
            _mockRepo = new Mock<IBookRepository>();
        }

        [Fact]
        public async Task Handle_Should_CallAddBookAsync_AndReturnDto()
        {
            // Arrange
            var handler = new CreateBookHandler(_mockRepo.Object);
            var command = new CreateBookCommand { Title = "Test Book", Author = "Tester" };

            var fakeBook = new Book
            {
                Id = 1,
                Title = command.Title,
                Author = command.Author,
                IsRead = false
            };

            _mockRepo
            .Setup(repo => repo.CreateBookAsync(It.IsAny<Book>()))
            .ReturnsAsync(1); // ✅ Return int because method returns Task<int>


            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            _mockRepo.Verify(repo => repo.CreateBookAsync(It.IsAny<Book>()), Times.Once);
            Assert.Equal(command.Title, result.Title);
            Assert.Equal(command.Author, result.Author);
        }
    }
}
