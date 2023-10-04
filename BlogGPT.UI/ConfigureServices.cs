using BlogGPT.Application.Common.Interfaces.Identity;
using BlogGPT.Infrastructure.Data;
using BlogGPT.UI.Services;
using System.Reflection;

namespace BlogGPT.UI
{
    public static class ConfigureServices
    {
        public static IServiceCollection AddUIServices(this IServiceCollection services)
        {
            services.AddAutoMapper(Assembly.GetExecutingAssembly());

            services.AddDatabaseDeveloperPageExceptionFilter();

            services.AddScoped<IUser, CurrentUser>();

            services.AddHttpContextAccessor();

            services.AddHealthChecks().AddDbContextCheck<ApplicationDbContext>();

            return services;
        }
    }
}
