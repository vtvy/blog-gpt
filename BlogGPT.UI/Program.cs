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
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.AddExceptionHandler();

app.Run();
