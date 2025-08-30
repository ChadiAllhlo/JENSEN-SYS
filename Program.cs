using api.Data;
using api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.AddCors();

builder.Services.AddIdentityApiEndpoints<User>(options =>
{
    options.User.RequireUniqueEmail = true;
})
.AddRoles<IdentityRole>()
.AddEntityFrameworkStores<AppDbContext>();

builder.Services.ConfigureApplicationCookie(options => 
{ 
    options.Cookie.SameSite = SameSiteMode.Lax; // Use Lax for better cross-origin support
    options.Cookie.SecurePolicy = CookieSecurePolicy.None; // Allow cookies on HTTP for development
    options.Cookie.Domain = null; // Allow cookies to be sent to different ports
    options.Cookie.HttpOnly = true; // Secure: prevent XSS attacks
    options.Cookie.IsEssential = true;
    options.ExpireTimeSpan = TimeSpan.FromDays(30); // Keep user logged in for 30 days
    options.SlidingExpiration = true; // Extend expiration on activity
});


var app = builder.Build();

// Pipeline
// ================================================================================
app.UseStaticFiles();

app.UseCors(c => c
    .AllowAnyHeader()
    .AllowAnyMethod()
    .AllowCredentials()
    .WithOrigins("http://localhost:3000", "https://localhost:3000", "http://127.0.0.1:5501", "https://127.0.0.1:5501", "http://127.0.0.1:5500", "https://127.0.0.1:5500", "https://localhost:5001", "http://localhost:5000")
);

app.UseAuthentication();
app.UseAuthorization();

app.MapGroup("api").MapIdentityApi<User>();
app.MapControllers();

// Serve the SPA for non-API routes only
app.MapFallback(context =>
{
    if (!context.Request.Path.StartsWithSegments("/api"))
    {
        context.Response.Redirect("/index.html");
    }
    return Task.CompletedTask;
});

// Seed dummy data...
// ================================================================================
using var scope = app.Services.CreateScope();
var services = scope.ServiceProvider;

try
{
    var context = services.GetRequiredService<AppDbContext>();
    var userManager = services.GetRequiredService<UserManager<User>>();
    await InitializeDb.SeedData(context, userManager);
}
catch (Exception ex)
{
    var logger = services.GetRequiredService<ILogger<Program>>();
    logger.LogError(ex, "Det gick fel vid migrering av databasen.");
}

app.Run();
