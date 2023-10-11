using BlogGPT.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BlogGPT.Infrastructure.Data.Configurations
{
    public class ModelConfiguration : IEntityTypeConfiguration<Model>
    {
        public void Configure(EntityTypeBuilder<Model> builder)
        {
            builder.Property(model => model.Name).HasMaxLength(Lengths.Large);

            builder.Property(model => model.LastModifiedBy).HasMaxLength(Lengths.XL);

            builder.HasMany(model => model.Messages).WithOne(message => message.Model).HasForeignKey(message => message.ModelId).OnDelete(DeleteBehavior.SetNull);
        }
    }
}