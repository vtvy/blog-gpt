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

            if (!_context.Categories.Any())
            {
                var defaultCategoryList = new List<Category>
                {
                    new ()
                    {
                        Name = "Arts and Entertainment",
                        Description = "Arts and Entertainment",
                        Slug = "arts-and-entertainment",
                    },
                    new ()
                    {
                        Name = "Cars & Other Vehicles",
                        Description = "Cars & Other Vehicles",
                        Slug = "cars-&-other-vehicles",
                    },
                    new ()
                    {
                        Name = "Computers and Electronics",
                        Description = "Computers and Electronics",
                        Slug = "computers-and-electronics",
                    },
                    new ()
                    {
                        Name = "Education and Communications",
                        Description = "Education and Communications",
                        Slug = "education-and-communications",
                    },
                    new ()
                    {
                        Name = "Family Life",
                        Description = "Family Life",
                        Slug = "family-life",
                    },
                    new ()
                    {
                        Name = "Finance and Business",
                        Description = "Finance and Business",
                        Slug = "finance-and-business",
                    },
                    new ()
                    {
                        Name = "Food and Entertaining",
                        Description = "Food and Entertaining",
                        Slug = "food-and-entertaining",
                    },
                    new ()
                    {
                        Name = "Health",
                        Description = "Health",
                        Slug = "health",
                    },
                    new ()
                    {
                        Name = "Hobbies and Crafts",
                        Description = "Hobbies and Crafts",
                        Slug = "hobbies-and-crafts",
                    },
                    new ()
                    {
                        Name = "Holidays and Traditions",
                        Description = "Holidays and Traditions",
                        Slug = "holidays-and-traditions",
                    },
                    new ()
                    {
                        Name = "Home and Garden",
                        Description = "Home and Garden",
                        Slug = "home-and-garden",
                    },
                    new ()
                    {
                        Name = "Personal Care and Style",
                        Description = "Personal Care and Style",
                        Slug = "personal-care-and-style",
                    },
                    new ()
                    {
                        Name = "Pets and Animals",
                        Description = "Pets and Animals",
                        Slug = "pets-and-animals",
                    },
                    new ()
                    {
                        Name = "Philosophy and Religion",
                        Description = "Philosophy and Religion",
                        Slug = "philosophy-and-religion",
                    },
                    new ()
                    {
                        Name = "Relationships",
                        Description = "Relationships",
                        Slug = "relationships",
                    },
                    new ()
                    {
                        Name = "Sports and Fitness",
                        Description = "Sports and Fitness",
                        Slug = "sports-and-fitness",
                    },
                    new ()
                    {
                        Name = "Travel",
                        Description = "Travel",
                        Slug = "travel",
                    },
                    new ()
                    {
                        Name = "Work World",
                        Description = "Work World",
                        Slug = "work-world",
                    },
                    new ()
                    {
                        Name = "Youth",
                        Description = "Youth",
                        Slug = "youth",
                    }
                };

                _context.Categories.AddRange(defaultCategoryList);

                await _context.SaveChangesAsync();
            }
        }
    }
}
