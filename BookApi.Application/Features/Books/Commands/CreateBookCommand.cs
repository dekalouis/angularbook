using BookApi.Dtos;
using MediatR;

namespace BookApi.Application.Features.Books.Commands
{
    public class CreateBookCommand : IRequest<BookResponseDto>  // returns new book ID
    {
        public string Title { get; set; } = string.Empty;
        public string Author { get; set; } = string.Empty;
        public int UserId { get; set; }
    }
}
