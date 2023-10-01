using BlogGPT.Domain.Constants;
using BlogGPT.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BlogGPT.Infrastructure.Data.Configurations
{
    public class ImageConfiguration : IEntityTypeConfiguration<Image>
    {
        public void Configure(EntityTypeBuilder<Image> builder)
        {
            builder.Property(i => i.Name).HasMaxLength(Lengths.Medium);
            builder.Property(i => i.Url).HasMaxLength(Lengths.Large);
        }
    }
}
