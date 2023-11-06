using BlogGPT.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BlogGPT.Infrastructure.Data.Configurations
{
    public class ViewPostConfiguration : IEntityTypeConfiguration<ViewPost>
    {
        public void Configure(EntityTypeBuilder<ViewPost> builder)
        {
            builder.Property(viewPost => viewPost.ViewerId).HasMaxLength(Lengths.XL);
        }
    }
}