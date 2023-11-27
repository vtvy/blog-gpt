using BlogGPT.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BlogGPT.Infrastructure.Data.Configurations
{
    public class CommentConfiguration : IEntityTypeConfiguration<Comment>
    {
        public void Configure(EntityTypeBuilder<Comment> builder)
        {
            builder.Property(comment => comment.Content).HasMaxLength(Lengths.XL);

            builder.Property(comment => comment.LastModifiedBy).HasMaxLength(Lengths.XL);

            //builder.Navigation(comment => comment.Author).AutoInclude();
        }
    }
}
