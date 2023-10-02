using BlogGPT.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BlogGPT.Infrastructure.Data.Configurations
{
    public class ImageConfiguration : IEntityTypeConfiguration<Image>
    {
        public void Configure(EntityTypeBuilder<Image> builder)
        {
            builder.Property(image => image.Name).HasMaxLength(Lengths.Medium);

            builder.Property(image => image.LastModifiedBy).HasMaxLength(Lengths.XL);

            builder.Property(image => image.Url).HasMaxLength(Lengths.Large);
        }
    }
}
