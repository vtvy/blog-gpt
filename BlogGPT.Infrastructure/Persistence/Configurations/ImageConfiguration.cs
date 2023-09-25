using BlogGPT.Domain.Constants;
using BlogGPT.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BlogGPT.Infrastructure.Persistence.Configurations
{
    public class ImageConfiguration : IEntityTypeConfiguration<Image>
    {
        public void Configure(EntityTypeBuilder<Image> builder)
        {
            builder.Property(i => i.Name).HasMaxLength(MaxLength.Medium);
            builder.Property(i => i.Url).HasMaxLength(MaxLength.Large);
        }
    }
}
