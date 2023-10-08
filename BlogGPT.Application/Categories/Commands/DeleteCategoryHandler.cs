using BlogGPT.Application.Common.Interfaces.Data;
using BlogGPT.Domain.Exceptions;

namespace BlogGPT.Application.Categories.Commands
{
    public record DeleteCategoryCommand : IRequest
    {
        public int Id { get; set; }
    }

    public class DeleteCategoryHandler : IRequestHandler<DeleteCategoryCommand>
    {
        private readonly IApplicationDbContext _context;

        public DeleteCategoryHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task Handle(DeleteCategoryCommand command, CancellationToken cancellationToken)
        {
            var entity = await _context.Categories.FindAsync(new object?[] { command.Id }, cancellationToken: cancellationToken) ?? throw new NotFoundException(nameof(Category), command.Id);

            _context.Categories.Remove(entity);

            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}