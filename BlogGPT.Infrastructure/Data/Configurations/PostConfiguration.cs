using BlogGPT.Domain.Constants;
using BlogGPT.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BlogGPT.Infrastructure.Data.Configurations
{
    public class PostConfiguration : IEntityTypeConfiguration<Post>
    {
        public void Configure(EntityTypeBuilder<Post> builder)
        {
            builder.Property(a => a.Title).HasMaxLength(Lengths.Medium);
            builder.Property(a => a.Description).HasMaxLength(Lengths.XL);
            builder.HasMany(a => a.PostCategories).WithOne(ac => ac.Post).HasForeignKey(ac => ac.PostId);
            builder.Navigation(a => a.Thumbnail).AutoInclude();
            builder.Navigation(a => a.PostCategories).AutoInclude();
        }
    }
}
