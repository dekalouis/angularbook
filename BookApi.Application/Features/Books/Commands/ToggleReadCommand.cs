using MediatR;

namespace BookApi.Application.Features.Books.Commands
{
    public class ToggleReadCommand : IRequest<bool?>
    {
        public int Id { get; set; }
        public int UserId { get; set; }
    }
}