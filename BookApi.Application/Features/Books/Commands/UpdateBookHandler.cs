using MediatR;
using BookApi.Application.Interfaces;
using BookApi.Domain.Entities;
// using System.Threading;
// using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace BookApi.Application.Features.Books.Commands
{
    public class UpdateBookHandler : IRequestHandler<UpdateBookCommand, Book?>
    {
        private readonly IBookRepository _repo;

        public UpdateBookHandler(IBookRepository repo)
        {
            _repo = repo;
        }

        public async Task<Book?> Handle(UpdateBookCommand request, CancellationToken cancellationToken)
        {

            if (string.IsNullOrWhiteSpace(request.Title))
                throw new ValidationException("Title cannot be empty.");

            if (string.IsNullOrWhiteSpace(request.Author))
                throw new ValidationException("Author cannot be empty.");

            var book = new Book
            {
                Id = request.Id,
                Title = request.Title,
                Author = request.Author,
                UserId = request.UserId
            };

            return await _repo.UpdateBookAsync(book);
        }

    }
}