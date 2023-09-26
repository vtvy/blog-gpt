using BlogGPT.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BlogGPT.Infrastructure.Persistence
{
    public class DbContext : IdentityDbContext<User>
    {
        public DbContext() { }
        public DbContext(DbContextOptions<DbContext> options) : base(options) { }

    }
}
