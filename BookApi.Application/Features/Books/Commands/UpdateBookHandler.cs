using MediatR;
using BookApi.Application.Interfaces;
using BookApi.Domain.Entities;
using System.Threading;
using System.Threading.Tasks;

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