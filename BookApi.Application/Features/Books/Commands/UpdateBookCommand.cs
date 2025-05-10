using MediatR;
using BookApi.Domain.Entities;

namespace BookApi.Application.Features.Books.Commands
{
    public class UpdateBookCommand : IRequest<Book?>
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Author { get; set; } = string.Empty;
    }
}