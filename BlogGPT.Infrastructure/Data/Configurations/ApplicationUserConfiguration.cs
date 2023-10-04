using BlogGPT.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BlogGPT.Infrastructure.Data.Configurations
{
    public class ApplicationUserConfiguration : IEntityTypeConfiguration<ApplicationUser>
    {
        public void Configure(EntityTypeBuilder<ApplicationUser> builder)
        {
            builder.Property(user => user.Avatar).HasMaxLength(Lengths.Large);

            builder.HasMany(user => user.Images).WithOne(image => image.Author).HasForeignKey(image => image.AuthorId);

            builder.HasMany(user => user.Categories).WithOne(category => category.Author).HasForeignKey(category => category.AuthorId);

            builder.HasMany(user => user.Posts).WithOne(post => post.Author).HasForeignKey(post => post.AuthorId);

            builder.HasMany(user => user.Comments).WithOne(comment => comment.Author).HasForeignKey(comment => comment.AuthorId);
        }
    }
}
