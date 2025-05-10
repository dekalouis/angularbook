using MediatR;
using BookApi.Domain.Entities;
using System.Collections.Generic;

namespace BookApi.Application.Features.Books.Queries
{
    public class GetUserBooksQuery : IRequest<List<Book>>
    {
        public int UserId { get; set; }
    }
}
