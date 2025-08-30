using api.Data;
using api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using System.Threading.RateLimiting;
using Microsoft.Extensions.FileProviders;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Configure request size limits
builder.Services.Configure<IISServerOptions>(options =>
{
    options.MaxRequestBodySize = 10 * 1024 * 1024; // 10MB limit
});

builder.Services.Configure<KestrelServerOptions>(options =>
{
    options.Limits.MaxRequestBodySize = 10 * 1024 * 1024; // 10MB limit
});

builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.AddCors();

// Add enhanced rate limiting
builder.Services.AddRateLimiter(options =>
{
    // Global rate limiter
    options.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(context =>
        RateLimitPartition.GetFixedWindowLimiter("GlobalLimiter",
            partition => new FixedWindowRateLimiterOptions
            {
                AutoReplenishment = true,
                PermitLimit = 100,
                Window = TimeSpan.FromMinutes(1)
            }));
});

builder.Services.AddIdentityApiEndpoints<User>(options =>
{
    options.User.RequireUniqueEmail = true;
    
    // Password requirements
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequireUppercase = true;
    options.Password.RequiredLength = 8;
    options.Password.RequiredUniqueChars = 1;
    
    // Lockout settings
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(15);
    options.Lockout.MaxFailedAccessAttempts = 5;
    options.Lockout.AllowedForNewUsers = true;
})
.AddRoles<IdentityRole>()
.AddEntityFrameworkStores<AppDbContext>();

builder.Services.ConfigureApplicationCookie(options => 
{ 
    options.Cookie.SameSite = SameSiteMode.Lax; // Use Lax for better cross-origin support
    options.Cookie.SecurePolicy = builder.Environment.IsDevelopment() 
        ? CookieSecurePolicy.None 
        : CookieSecurePolicy.Always; // Secure in production
    options.Cookie.Domain = null; // Allow cookies to be sent to different ports
    options.Cookie.HttpOnly = true; // Secure: prevent XSS attacks
    options.Cookie.IsEssential = true;
    options.ExpireTimeSpan = TimeSpan.FromDays(30); // Keep user logged in for 30 days
    options.SlidingExpiration = true; // Extend expiration on activity
});


var app = builder.Build();

// Pipeline
// ================================================================================

// Add enhanced security headers
app.Use(async (context, next) =>
{
    // Content Security Policy (Enhanced)
    context.Response.Headers["Content-Security-Policy"] = 
        "default-src 'self'; " +
        "script-src 'self' 'unsafe-inline'; " +
        "style-src 'self' 'unsafe-inline'; " +
        "img-src 'self' data:; " +
        "font-src 'self'; " +
        "connect-src 'self'; " +
        "frame-ancestors 'none'; " +
        "base-uri 'self'; " +
        "form-action 'self'; " +
        "upgrade-insecure-requests;";

    // X-Content-Type-Options
    context.Response.Headers["X-Content-Type-Options"] = "nosniff";
    
    // X-Frame-Options
    context.Response.Headers["X-Frame-Options"] = "DENY";
    
    // X-XSS-Protection
    context.Response.Headers["X-XSS-Protection"] = "1; mode=block";
    
    // Referrer Policy
    context.Response.Headers["Referrer-Policy"] = "strict-origin-when-cross-origin";
    
    // Permissions Policy (Enhanced)
    context.Response.Headers["Permissions-Policy"] = 
        "camera=(), microphone=(), geolocation=(), payment=(), usb=(), magnetometer=(), gyroscope=()";
    
    // Additional Security Headers
    context.Response.Headers["Strict-Transport-Security"] = "max-age=31536000; includeSubDomains";
    context.Response.Headers["X-Permitted-Cross-Domain-Policies"] = "none";

    await next();
});

app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(
        Path.Combine(Directory.GetCurrentDirectory(), "..", "js-client")),
    RequestPath = ""
});

app.UseCors(c => c
    .WithOrigins("http://localhost:3000", "https://localhost:3000", "http://127.0.0.1:5501", "https://127.0.0.1:5501", "http://127.0.0.1:5500", "https://127.0.0.1:5500", "https://localhost:5001", "http://localhost:5000")
    .AllowCredentials()
    .WithMethods("GET", "POST", "PUT", "DELETE", "OPTIONS")
    .WithHeaders("Content-Type", "Authorization")
);

app.UseAuthentication();
app.UseAuthorization();

// Enable rate limiting
app.UseRateLimiter();

app.MapGroup("api").MapIdentityApi<User>();
app.MapControllers();

// Serve the SPA for non-API routes only
app.MapFallback(context =>
{
    if (!context.Request.Path.StartsWithSegments("/api"))
    {
        var indexPath = Path.Combine(Directory.GetCurrentDirectory(), "..", "js-client", "index.html");
        if (System.IO.File.Exists(indexPath))
        {
            context.Response.Redirect("/index.html");
        }
        else
        {
            context.Response.StatusCode = 404;
        }
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
    var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
    await InitializeDb.SeedData(context, userManager, roleManager);
}
catch (Exception ex)
{
    var logger = services.GetRequiredService<ILogger<Program>>();
    logger.LogError(ex, "Det gick fel vid migrering av databasen.");
}

app.Run();
