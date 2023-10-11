using BlogGPT.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BlogGPT.Infrastructure.Data.Configurations
{
    public class ConversationConfiguration : IEntityTypeConfiguration<Conversation>
    {
        public void Configure(EntityTypeBuilder<Conversation> builder)
        {
            builder.HasMany(conversation => conversation.Messages).WithOne(message => message.Conversation).HasForeignKey(message => message.ConversationId).OnDelete(DeleteBehavior.Cascade);

            builder.Property(conversation => conversation.LastModifiedBy).HasMaxLength(Lengths.XL);
        }
    }
}