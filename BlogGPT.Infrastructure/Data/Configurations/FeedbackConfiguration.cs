using BlogGPT.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BlogGPT.Infrastructure.Data.Configurations
{
    public class FeedbackConfiguration : IEntityTypeConfiguration<Feedback>
    {
        public void Configure(EntityTypeBuilder<Feedback> builder)
        {
            builder.Property(feedback => feedback.Content).HasMaxLength(Lengths.XL);

            builder.Property(feedback => feedback.LastModifiedBy).HasMaxLength(Lengths.XL);

            builder.Navigation(feedback => feedback.Author).AutoInclude();
        }
    }
}
