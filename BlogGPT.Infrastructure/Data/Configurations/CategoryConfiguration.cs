using BlogGPT.Domain.Constants;
using BlogGPT.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BlogGPT.Infrastructure.Data.Configurations
{
    public class CategoryConfiguration : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.Property(c => c.Name).HasMaxLength(Lengths.Medium);
            builder.Property(c => c.Slug).HasMaxLength(Lengths.Large);
            builder.HasMany(c => c.ChildrenCategories).WithOne(c => c.Parent).HasForeignKey(c => c.ParentId);
            builder.HasMany(c => c.PostCategories).WithOne(ac => ac.Category).HasForeignKey(c => c.CategoryId);
            builder.Navigation(c => c.PostCategories).AutoInclude();
        }
    }
}
