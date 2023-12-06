using BlogGPT.Application;
using BlogGPT.Infrastructure;
using BlogGPT.UI;
using BlogGPT.UI.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add console to view logs
builder.Logging.AddConsole();

// Add services to the container.
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddApplication();
builder.Services.AddUIServices();
builder.Services.AddControllersWithViews();

var app = builder.Build();

await app.InitializeDatabaseAsync();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHealthChecks("/health");
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllerRoute(
    name: "BlogArea",
    pattern: "{area:exists}/{controller=ManagePosts}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.AddExceptionHandler();

app.UseStatusCodePages(handleError =>
{
    handleError.Run(async context =>
    {
        await context.Response.WriteAsync(
            @$"<html lang=""en"">
                    <head>
                        <meta charset=""UTF-8"" />
                        <meta http-equiv=""X-UA-Compatible"" content=""IE=edge"" />
                        <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"" />
                        <title>{context.Response.StatusCode} Error</title>
                        <link href=""https://cdn.jsdelivr.net/npm/bootstrap@5.1.3/dist/css/bootstrap.min.css"" rel=""stylesheet"">
                    </head>
                    <body>
                        <div class=""d-flex align-items-center justify-content-center vh-100"">
                            <div class=""text-center"">
                                <h1 class=""display-1 fw-bold"">{context.Response.StatusCode}</h1>
                                <p class=""fs-3""> <span class=""text-danger"">Opps!</span> Some thing went wrong.</p>
                                <a href=""/"" class=""btn btn-primary"">Go Home</a>
                            </div>
                        </div>
                    </body>
               </html>"
            );
    });
});

app.Run();
