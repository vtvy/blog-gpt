using BlogGPT.Application.Common.Interfaces.Data;
using BlogGPT.Application.Common.Interfaces.Identity;
using BlogGPT.Application.Common.Interfaces.Services;
using BlogGPT.Domain.Constants;
using BlogGPT.Domain.Entities;
using BlogGPT.Infrastructure.Data;
using BlogGPT.Infrastructure.Data.Interceptors;
using BlogGPT.Infrastructure.Identity;
using BlogGPT.Infrastructure.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BlogGPT.Infrastructure
{
    public static class ConfigureServices
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<IDateTime, DateTimeService>();

            services.Configure<MailSettings>(configuration.GetSection("MailSettings"));

            services.AddTransient<IEmailSender, MailService>();

            var connectionString = configuration.GetConnectionString("Default") ?? throw new Exception("Connection string not found");


            services.AddScoped<ISaveChangesInterceptor, AuditableEntityInterceptor>();
            services.AddScoped<ISaveChangesInterceptor, DispatchDomainEventsInterceptor>();

            services.AddDbContext<ApplicationDbContext>((sp, options) =>
            {
                options.AddInterceptors(sp.GetServices<ISaveChangesInterceptor>());

                options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
            });

            services.AddScoped<IApplicationDbContext>(provider => provider.GetRequiredService<ApplicationDbContext>());

            services.AddScoped<ApplicationDbContextInitializer>();

            services.AddAuthentication();

            services.AddAuthorization();

            services.AddIdentity<ApplicationUser, IdentityRole>().AddEntityFrameworkStores<ApplicationDbContext>().AddDefaultTokenProviders();

            services.AddTransient<IIdentityService, IdentityService>();

            services.AddAuthorization(options => options.AddPolicy(Policies.CanManageCategory, policy => policy.RequireRole(Roles.Administrator)));

            return services;
        }
    }
}
