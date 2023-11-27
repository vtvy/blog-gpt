using BlogGPT.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BlogGPT.Infrastructure.Data.Configurations
{
    public class EmbeddingPostConfiguration : IEntityTypeConfiguration<EmbeddingPost>
    {
        public void Configure(EntityTypeBuilder<EmbeddingPost> builder)
        {
            builder.HasMany(embeddingPost => embeddingPost.EmbeddingChunks).WithOne(embeddingChunk => embeddingChunk.EmbeddingPost).HasForeignKey(message => message.EmbeddingPostId).OnDelete(DeleteBehavior.Cascade);
        }
    }
}