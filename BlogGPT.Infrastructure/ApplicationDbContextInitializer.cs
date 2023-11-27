using BlogGPT.Domain.Entities;
using BlogGPT.Infrastructure.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace BlogGPT.Infrastructure
{
    public static class InitializerExtensions
    {
        public static async Task InitializeDatabaseAsync(this WebApplication app)
        {
            using var scope = app.Services.CreateScope();

            var initializer = scope.ServiceProvider.GetRequiredService<ApplicationDbContextInitializer>();

            await initializer.InitializeAsync();

            await initializer.SeedAsync();
        }
    }

    public class ApplicationDbContextInitializer
    {
        private readonly ILogger<ApplicationDbContextInitializer> _logger;
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;

        public ApplicationDbContextInitializer(
            ILogger<ApplicationDbContextInitializer> logger,
            ApplicationDbContext context, UserManager<ApplicationUser> userManager
            , RoleManager<IdentityRole> roleManager,
            IConfiguration configuration)
        {
            _logger = logger;
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
        }

        public async Task InitializeAsync()
        {
            try
            {
                await _context.Database.MigrateAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while initializing the database.");
                throw;
            }
        }

        public async Task SeedAsync()
        {
            try
            {
                await TrySeedAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while seeding the database.");
                throw;
            }
        }

        public async Task TrySeedAsync()
        {
            // Default roles
            var administratorRole = new IdentityRole(Roles.Administrator);
            var editorRole = new IdentityRole(Roles.Editor);
            var userRole = new IdentityRole(Roles.User);

            if (!_roleManager.Roles.Any())
            {
                await _roleManager.CreateAsync(administratorRole);
                await _roleManager.CreateAsync(editorRole);
                await _roleManager.CreateAsync(userRole);
            }

            // Default users
            var administrator = new ApplicationUser { UserName = _configuration["Identity:UserName"], Email = _configuration["Identity:Email"] };
            var editor = new ApplicationUser { UserName = _configuration["Identity:UserName1"], Email = _configuration["Identity:Email1"] };
            var user = new ApplicationUser { UserName = _configuration["Identity:UserName2"], Email = _configuration["Identity:Email2"] };
            if (_userManager.Users.All(u => u.UserName != administrator.UserName))
            {
                var password = _configuration["Identity:Password"] ?? throw new Exception("Default password not found");
                await _userManager.CreateAsync(administrator, password);
                if (!string.IsNullOrWhiteSpace(administratorRole.Name))
                {
                    await _userManager.AddToRoleAsync(administrator, administratorRole.Name);
                }
            }
            if (_userManager.Users.All(u => u.UserName != editor.UserName))
            {
                var password1 = _configuration["Identity:Password1"] ?? throw new Exception("Default password not found");
                var result = await _userManager.CreateAsync(editor, password1);
                if (!string.IsNullOrWhiteSpace(editorRole.Name))
                {
                    await _userManager.AddToRoleAsync(editor, editorRole.Name);
                }
            }
            if (_userManager.Users.All(u => u.UserName != user.UserName))
            {
                var password2 = _configuration["Identity:Password2"] ?? throw new Exception("Default password not found");
                var result = await _userManager.CreateAsync(user, password2);
                if (!string.IsNullOrWhiteSpace(userRole.Name))
                {
                    await _userManager.AddToRoleAsync(user, userRole.Name);
                }
            }
        }
    }
}
