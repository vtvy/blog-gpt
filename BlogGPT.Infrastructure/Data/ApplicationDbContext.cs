using BlogGPT.Application.Common.Interfaces.Data;
using BlogGPT.Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace BlogGPT.Infrastructure.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>, IApplicationDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<Post> Posts => Set<Post>();

        public DbSet<EmbeddingPost> EmbeddingPosts => Set<EmbeddingPost>();

        public DbSet<EmbeddingChunk> EmbeddingChunks => Set<EmbeddingChunk>();

        public DbSet<Category> Categories => Set<Category>();

        public DbSet<Comment> Comments => Set<Comment>();

        public DbSet<PostCategory> PostCategories => Set<PostCategory>();

        public DbSet<Message> Messages => Set<Message>();

        public DbSet<View> Views => Set<View>();

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            base.OnModelCreating(builder);
        }
    }
}
