using BlogGPT.Application.Common.Interfaces.Data;
using BlogGPT.Application.Common.Interfaces.Identity;

namespace BlogGPT.Application.Categories.Commands
{
    public record DeleteCommentCommand : IRequest<int>
    {
        public int Id { get; set; }
    }

    public class DeleteCommentHandler(IApplicationDbContext context, IUser user) : IRequestHandler<DeleteCommentCommand, int>
    {
        private readonly IApplicationDbContext _context = context;
        private readonly IUser _user = user;

        public async Task<int> Handle(DeleteCommentCommand command, CancellationToken cancellationToken)
        {
            var existedPost = await _context.Comments.Where(c => c.Id == command.Id && c.AuthorId == _user.Id).FirstOrDefaultAsync(cancellationToken);

            if (existedPost == null) return -1;

            _context.Comments.Remove(existedPost);

            await _context.SaveChangesAsync(cancellationToken);

            return command.Id;
        }
    }
}
