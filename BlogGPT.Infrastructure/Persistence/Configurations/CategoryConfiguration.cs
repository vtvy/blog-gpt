using BlogGPT.Domain.Constants;
using BlogGPT.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BlogGPT.Infrastructure.Persistence.Configurations
{
    public class CategoryConfiguration : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.Property(c => c.Name).HasMaxLength(MaxLength.Medium);
            builder.Property(c => c.Slug).HasMaxLength(MaxLength.Large);
            builder.HasMany(c => c.ChildrenCategories).WithOne(c => c.Parent).HasForeignKey(c => c.ParentId);
            builder.HasMany(c => c.ArticleCategories).WithOne(ac => ac.Category).HasForeignKey(c => c.CategoryId);
            builder.Navigation(c => c.ArticleCategories).AutoInclude();
        }
    }
}
