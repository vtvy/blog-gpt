using BlogGPT.Application.Common.Interfaces.Services;
using BlogGPT.Infrastructure.Services;
using Microsoft.Extensions.DependencyInjection;

namespace BlogGPT.Infrastructure
{
    public static class ConfigureServices
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {
            services.AddSingleton<IDateTime, DateTimeService>();

            return services;
        }
    }
}
