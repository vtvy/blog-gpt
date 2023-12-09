using BlogGPT.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BlogGPT.Infrastructure.Data.Configurations
{
    public class EmbeddingPostConfiguration : IEntityTypeConfiguration<EmbeddingPost>
    {
        public void Configure(EntityTypeBuilder<EmbeddingPost> builder)
        {
        }
    }
}