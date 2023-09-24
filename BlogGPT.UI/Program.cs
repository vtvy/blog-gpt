using BlogGPT.Application;
using BlogGPT.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddInfrastructure();
builder.Services.AddApplication();
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

//app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
   name: "default",
   pattern: "{controller=Home}/{action=Index}/{id?}");

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
                    <title>Bootstrap 5 404 Error Page</title>
                    <link href=""/lib/bootstrap/dist/css/bootstrap.min.css"" rel=""stylesheet"">
                </head>
                <body>
                    <div class=""d-flex align-items-center justify-content-center vh-100"">
                        <div class=""text-center"">
                            <h1 class=""display-1 fw-bold"">404</h1>
                            <p class=""fs-3""> <span class=""text-danger"">Opps!</span> Page not found.</p>
                            <p class=""lead"">
                                The page you’re looking for does not exist.
                              </p>
                            <a href=""/"" class=""btn btn-primary"">Go Home</a>
                        </div>
                    </div>
                </body>
               </html>"
            );
    });
});

app.Run();
