using BlogGPT.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BlogGPT.Infrastructure.Data.Configurations
{
    public class PostConfiguration : IEntityTypeConfiguration<Post>
    {
        public void Configure(EntityTypeBuilder<Post> builder)
        {
            builder.Property(post => post.Title).HasMaxLength(Lengths.Medium);

            builder.Property(post => post.Description).HasMaxLength(Lengths.XL);

            builder.Property(post => post.LastModifiedBy).HasMaxLength(Lengths.XL);

            builder.Property(post => post.Slug).HasMaxLength(Lengths.Large);
            builder.HasIndex(post => post.Slug).IsUnique();

            builder.HasMany(post => post.PostCategories).WithOne(ac => ac.Post).HasForeignKey(ac => ac.PostId);

            builder.Navigation(post => post.Thumbnail).AutoInclude();

            builder.Navigation(post => post.Author).AutoInclude();

            builder.Navigation(post => post.PostCategories).AutoInclude();
        }
    }
}
