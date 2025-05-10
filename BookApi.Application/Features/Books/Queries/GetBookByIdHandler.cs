using MediatR;
using BookApi.Application.Interfaces;
using BookApi.Domain.Entities;
using System.Threading;
using System.Threading.Tasks;

namespace BookApi.Application.Features.Books.Queries
{
    public class GetBookByIdHandler : IRequestHandler<GetBookByIdQuery, Book?>
    {
        private readonly IBookRepository _repo;

        public GetBookByIdHandler(IBookRepository repo)
        {
            _repo = repo;
        }

        public async Task<Book?> Handle(GetBookByIdQuery request, CancellationToken cancellationToken)
        {
            return await _repo.GetBookByIdAsync(request.Id, request.UserId);
        }
    }
}
