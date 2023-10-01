using BlogGPT.Application.Common.Interfaces.Identity;
using BlogGPT.Infrastructure.Data;
using BlogGPT.UI.Services;

namespace BlogGPT.UI
{
    public static class ConfigureServices
    {
        public static IServiceCollection AddUIServices(this IServiceCollection services)
        {
            services.AddDatabaseDeveloperPageExceptionFilter();

            services.AddScoped<IUser, CurrentUser>();

            services.AddHttpContextAccessor();

            services.AddHealthChecks().AddDbContextCheck<ApplicationDbContext>();

            return services;
        }
    }
}
