using BlogGPT.Application.Common.Interfaces.Data;

namespace BlogGPT.Application.Categories.Commands
{
    public record CreateCommentCommand : IRequest<int>
    {
        public int PostId { get; set; }

        public required string Comment { get; set; }
    }

    public class CreateCommentHandler(IApplicationDbContext context) : IRequestHandler<CreateCommentCommand, int>
    {
        private readonly IApplicationDbContext _context = context;

        public async Task<int> Handle(CreateCommentCommand command, CancellationToken cancellationToken)
        {
            var existedPost = await _context.Posts.FindAsync([command.PostId], cancellationToken: cancellationToken);

            if (existedPost == null) return -1;

            var entity = new Comment { Content = command.Comment, PostId = command.PostId };

            await _context.Comments.AddAsync(entity, cancellationToken);

            await _context.SaveChangesAsync(cancellationToken);

            return entity.Id;
        }
    }
}
