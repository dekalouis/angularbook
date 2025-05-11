using MediatR;
using BookApi.Domain.Entities;
using BookApi.Application.Interfaces;
using BookApi.Dtos;
using System.ComponentModel.DataAnnotations;

namespace BookApi.Application.Features.Books.Commands
{
    public class CreateBookHandler : IRequestHandler<CreateBookCommand, BookResponseDto>

    {
        private readonly IBookRepository _repo;

        public CreateBookHandler(IBookRepository repo)
        {
            _repo = repo;
        }

        public async Task<BookResponseDto> Handle(CreateBookCommand request, CancellationToken cancellationToken)

        {

            if (string.IsNullOrWhiteSpace(request.Title))
                throw new ValidationException("Title cannot be empty.");

            if (string.IsNullOrWhiteSpace(request.Author))
                throw new ValidationException("Author cannot be empty.");


            var book = new Book
            {
                Title = request.Title,
                Author = request.Author,
                UserId = request.UserId,
                IsRead = false
            };

            await _repo.CreateBookAsync(book);
            // return book.Id;
            return new BookResponseDto
            {
                Id = book.Id,
                Title = book.Title,
                Author = book.Author,
                IsRead = book.IsRead
            };

        }
    }
}
