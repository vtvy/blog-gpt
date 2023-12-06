using BlogGPT.Application.Common.Interfaces.Data;
using BlogGPT.Application.Common.Interfaces.Identity;

namespace BlogGPT.Application.Categories.Commands
{
    public record EditCommentCommand : IRequest<int>
    {
        public int PostId { get; set; }

        public int Id { get; set; }

        public required string Comment { get; set; }
    }

    public class EditCommentHandler(IApplicationDbContext context, IUser user) : IRequestHandler<EditCommentCommand, int>
    {
        private readonly IApplicationDbContext _context = context;
        private readonly IUser _user = user;

        public async Task<int> Handle(EditCommentCommand command, CancellationToken cancellationToken)
        {
            var existedPost = await _context.Comments.Where(c => c.PostId == command.PostId && c.Id == command.Id && c.AuthorId == _user.Id).FirstOrDefaultAsync(cancellationToken);

            if (existedPost == null) return -1;

            existedPost.Content = command.Comment;

            _context.Comments.Update(existedPost);

            await _context.SaveChangesAsync(cancellationToken);

            return existedPost.Id;
        }
    }
}
