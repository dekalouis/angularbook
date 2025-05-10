using MediatR;
using BookApi.Application.Interfaces;
using BookApi.Domain.Entities;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace BookApi.Application.Features.Books.Queries
{
    public class GetUserBooksHandler : IRequestHandler<GetUserBooksQuery, List<Book>>
    {
        private readonly IBookRepository _repo;

        public GetUserBooksHandler(IBookRepository repo)
        {
            _repo = repo;
        }

        public async Task<List<Book>> Handle(GetUserBooksQuery request, CancellationToken cancellationToken)
        {
            return await _repo.GetBooksByUserIdAsync(request.UserId);
        }
    }
}
