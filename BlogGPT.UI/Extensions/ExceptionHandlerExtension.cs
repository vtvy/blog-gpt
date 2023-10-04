using BlogGPT.UI.Services;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http.Features;

namespace BlogGPT.UI.Extensions
{
    public static class ExceptionHandlerExtension
    {
        public static void AddExceptionHandler(this IApplicationBuilder app)
        {
            app.UseExceptionHandler(configure => configure.Run(async context =>
            {
                var exception = context.Features.GetRequiredFeature<IExceptionHandlerPathFeature>().Error;
                var exceptionHandler = new CustomExceptionHandler();
                await exceptionHandler.TryHandleAsync(context, exception);
            }));
        }
    }
}
