using BlogGPT.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BlogGPT.Infrastructure.Data.Configurations
{
    public class CategoryConfiguration : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.Property(category => category.Name).HasMaxLength(Lengths.Medium);

            builder.Property(category => category.LastModifiedBy).HasMaxLength(Lengths.XL);

            builder.Property(category => category.Description).HasMaxLength(Lengths.XL);

            builder.Property(category => category.Slug).HasMaxLength(Lengths.Large);
            builder.HasIndex(category => category.Slug).IsUnique();

            builder.HasMany(category => category.ChildrenCategories).WithOne(category => category.Parent).HasForeignKey(category => category.ParentId);

            builder.HasMany(category => category.PostCategories).WithOne(postCategory => postCategory.Category).HasForeignKey(category => category.CategoryId);

            builder.Navigation(category => category.PostCategories).AutoInclude();
        }
    }
}
