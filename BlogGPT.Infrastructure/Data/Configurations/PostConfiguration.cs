﻿using BlogGPT.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BlogGPT.Infrastructure.Data.Configurations
{
    public class PostConfiguration : IEntityTypeConfiguration<Post>
    {
        public void Configure(EntityTypeBuilder<Post> builder)
        {
            builder.Property(post => post.Title).HasMaxLength(Lengths.XL);

            builder.Property(post => post.Description).HasMaxLength(Lengths.XL);

            builder.Property(post => post.LastModifiedBy).HasMaxLength(Lengths.XL);

            builder.Property(post => post.Slug).HasMaxLength(Lengths.Large);
            builder.HasIndex(post => post.Slug).IsUnique();

            builder.HasOne(post => post.EmbeddingPost).WithOne(embeddingPost => embeddingPost.Post).HasForeignKey<EmbeddingPost>(embeddingPost => embeddingPost.PostId).OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(post => post.PostCategories).WithOne(postCategory => postCategory.Post).HasForeignKey(postCategory => postCategory.PostId);

            builder.HasMany(post => post.Comments).WithOne(comment => comment.Post).HasForeignKey(ac => ac.PostId).OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(post => post.ViewPosts).WithOne(viewPost => viewPost.Post).HasForeignKey(vp => vp.PostId).OnDelete(DeleteBehavior.Cascade);

            //builder.Navigation(post => post.Thumbnail).AutoInclude();

            //builder.Navigation(post => post.Author).AutoInclude();

            //builder.Navigation(post => post.PostCategories).AutoInclude();
        }
    }
}
