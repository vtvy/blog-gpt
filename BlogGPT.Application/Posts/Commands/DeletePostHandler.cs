using BlogGPT.Application.Common.Interfaces.Data;
using BlogGPT.Domain.Exceptions;

namespace BlogGPT.Application.Posts.Commands
{
    public record DeletePostCommand : IRequest
    {
        public int Id { get; set; }
    }

    public class DeletePostHandler : IRequestHandler<DeletePostCommand>
    {
        private readonly IApplicationDbContext _context;
        public DeletePostHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task Handle(DeletePostCommand command, CancellationToken cancellationToken)
        {
            var entity = await _context.Posts.FindAsync(new { command.Id }, cancellationToken);

            if (entity == null)
            {
                throw new NotFoundException(nameof(Post), command.Id);
            }

            _context.Posts.Remove(entity);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}