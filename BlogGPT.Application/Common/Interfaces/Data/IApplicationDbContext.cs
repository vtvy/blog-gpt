namespace BlogGPT.Application.Common.Interfaces.Data
{
    public interface IApplicationDbContext
    {
        DbSet<Post> Posts { get; }

        DbSet<EmbeddingPost> EmbeddingPosts { get; }

        DbSet<EmbeddingChunk> EmbeddingChunks { get; }

        DbSet<Category> Categories { get; }

        DbSet<Comment> Comments { get; }

        DbSet<PostCategory> PostCategories { get; }

        DbSet<Message> Messages { get; }

        DbSet<View> Views { get; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
