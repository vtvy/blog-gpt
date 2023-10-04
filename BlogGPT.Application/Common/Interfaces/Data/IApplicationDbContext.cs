using BlogGPT.Domain.Entities;

namespace BlogGPT.Application.Common.Interfaces.Data
{
    public interface IApplicationDbContext
    {
        DbSet<Post> Posts { get; }

        DbSet<Category> Categories { get; }

        DbSet<Image> Images { get; }

        DbSet<Comment> Feedbacks { get; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}
