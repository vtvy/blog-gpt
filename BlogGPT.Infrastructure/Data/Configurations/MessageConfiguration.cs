using BlogGPT.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BlogGPT.Infrastructure.Data.Configurations
{
    public class MessageConfiguration : IEntityTypeConfiguration<Message>
    {
        public void Configure(EntityTypeBuilder<Message> builder)
        {
            builder.Property(message => message.Question).HasMaxLength(Lengths.XXL);

            builder.Property(message => message.LastModifiedBy).HasMaxLength(Lengths.XL);
        }
    }
}