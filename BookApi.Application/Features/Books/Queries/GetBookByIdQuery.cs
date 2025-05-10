using MediatR;
using BookApi.Domain.Entities;

namespace BookApi.Application.Features.Books.Queries
{
    public class GetBookByIdQuery : IRequest<Book?>
    {
        public int Id { get; set; }
        public int UserId { get; set; }
    }
}
