using BlogGPT.Domain.Constants;
using BlogGPT.Infrastructure.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BlogGPT.Infrastructure.Persistence.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.Property(u => u.Avatar).HasMaxLength(MaxLength.Large);
            builder.Property(u => u.Role).HasMaxLength(MaxLength.Medium);
            builder.HasMany(u => u.Images).WithOne(i => i.Author).HasForeignKey(i => i.AuthorId);
            builder.HasMany(u => u.Categories).WithOne(c => c.Author).HasForeignKey(c => c.AuthorId);
            builder.HasMany(u => u.Articles).WithOne(a => a.Author).HasForeignKey(a => a.AuthorId);
            builder.HasMany(u => u.Feedbacks).WithOne(f => f.Author).HasForeignKey(f => f.AuthorId);
        }
    }
}
