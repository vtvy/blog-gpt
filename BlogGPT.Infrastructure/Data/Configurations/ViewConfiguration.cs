using BlogGPT.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BlogGPT.Infrastructure.Data.Configurations
{
    public class ViewConfiguration : IEntityTypeConfiguration<View>
    {
        public void Configure(EntityTypeBuilder<View> builder)
        {
        }
    }
}