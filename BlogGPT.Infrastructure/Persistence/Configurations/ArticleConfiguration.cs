using BlogGPT.Domain.Constants;
using BlogGPT.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BlogGPT.Infrastructure.Persistence.Configurations
{
    public class ArticleConfiguration : IEntityTypeConfiguration<Article>
    {
        public void Configure(EntityTypeBuilder<Article> builder)
        {
            builder.Property(a => a.Title).HasMaxLength(MaxLength.Medium);
            builder.Property(a => a.Description).HasMaxLength(MaxLength.XL);
            builder.HasMany(a => a.ArticleCategories).WithOne(ac => ac.Article).HasForeignKey(ac => ac.ArticleId);
            builder.Navigation(a => a.Thumbnail).AutoInclude();
            builder.Navigation(a => a.Author).AutoInclude();
            builder.Navigation(a => a.ArticleCategories).AutoInclude();
        }
    }
}
