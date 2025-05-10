using MediatR;
using BookApi.Application.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace BookApi.Application.Features.Books.Commands
{
    public class DeleteBookHandler : IRequestHandler<DeleteBookCommand, bool>
    {
        private readonly IBookRepository _repo;

        public DeleteBookHandler(IBookRepository repo)
        {
            _repo = repo;
        }

        public async Task<bool> Handle(DeleteBookCommand request, CancellationToken cancellationToken)
        {
            return await _repo.DeleteBookAsync(request.Id, request.UserId);
        }
    }
}
