namespace BlogGPT.Application.Common.Interfaces.Data
{
    public interface IApplicationDbContext
    {
        DbSet<Post> Posts { get; }

        DbSet<EmbeddingPost> EmbeddingPosts { get; }

        DbSet<Category> Categories { get; }

        DbSet<Image> Images { get; }

        DbSet<Comment> Comments { get; }

        DbSet<PostCategory> PostCategories { get; }

        DbSet<Conversation> Conversations { get; }

        DbSet<Message> Messages { get; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}
