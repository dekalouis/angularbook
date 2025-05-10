using MediatR;
using BookApi.Application.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace BookApi.Application.Features.Books.Commands
{
    public class ToggleReadHandler : IRequestHandler<ToggleReadCommand, bool?>
    {
        private readonly IBookRepository _repo;

        public ToggleReadHandler(IBookRepository repo)
        {
            _repo = repo;
        }

        public async Task<bool?> Handle(ToggleReadCommand request, CancellationToken cancellationToken)
        {
            return await _repo.ToggleReadAsync(request.Id, request.UserId);
        }
    }
}
