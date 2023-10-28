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

        public DbSet<Category> Categories => Set<Category>();

        public DbSet<Image> Images => Set<Image>();

        public DbSet<Comment> Comments => Set<Comment>();

        public DbSet<PostCategory> PostCategories => Set<PostCategory>();

        public DbSet<Conversation> Conversations => Set<Conversation>();

        public DbSet<Message> Messages => Set<Message>();

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            base.OnModelCreating(builder);
        }
    }
}
