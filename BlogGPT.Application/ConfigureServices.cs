using BlogGPT.Application.Chats;
using BlogGPT.Application.Common.Behaviors;
using BlogGPT.Application.Images;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace BlogGPT.Application
{
    public static class ConfigureServices
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddAutoMapper(Assembly.GetExecutingAssembly());

            services.AddMediatR(configuration =>
            {
                configuration.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
                configuration.AddBehavior(typeof(IPipelineBehavior<,>), typeof(UnhandledExceptionBehavior<,>));
                configuration.AddBehavior(typeof(IPipelineBehavior<,>), typeof(AuthorizationBehavior<,>));
                configuration.AddBehavior(typeof(IPipelineBehavior<,>), typeof(PerformanceBehavior<,>));
            });

            services.AddScoped<IChatService, ChatService>();

            services.AddScoped<IImageService, ImageService>();

            return services;
        }
    }
}
