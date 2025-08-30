using api.Data;
using api.Models;
using api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers(options =>
{
    var policy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();
    options.Filters.Add(new AuthorizeFilter(policy));
});

builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.AddCors();

builder.Services.AddIdentityCore<User>(options =>
{
    options.User.RequireUniqueEmail = true;
})
.AddRoles<IdentityRole>()
.AddEntityFrameworkStores<AppDbContext>()
.AddSignInManager();

// JWT options
builder.Services.Configure<JwtOptions>(builder.Configuration.GetSection("Jwt"));

// Token service
builder.Services.AddScoped<ITokenService, TokenService>();

// Authentication - JWT Bearer
var jwtSection = builder.Configuration.GetSection("Jwt");
var issuer = jwtSection.GetValue<string>("Issuer") ?? "";
var audience = jwtSection.GetValue<string>("Audience") ?? "";
var secret = jwtSection.GetValue<string>("SecretKey") ?? "";

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = true;
    options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateIssuerSigningKey = true,
        ValidateLifetime = true,
        ValidIssuer = issuer,
        ValidAudience = audience,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret))
    };
});

// Basic request throttling could be added with ASP.NET built-ins or middleware later

// Security headers middleware registration happens in pipeline


var app = builder.Build();

// Pipeline
// ================================================================================
app.UseCors(c => c
    .AllowAnyHeader()
    .AllowAnyMethod()
    .WithOrigins("http://localhost:3000", "https://localhost:3000", "http://127.0.0.1:5501", "https://127.0.0.1:5501")
);

// No explicit rate limiter without package

app.Use(async (context, next) =>
{
    // Basic security headers
    context.Response.Headers["X-Content-Type-Options"] = "nosniff";
    context.Response.Headers["X-Frame-Options"] = "DENY";
    context.Response.Headers["Referrer-Policy"] = "no-referrer";
    context.Response.Headers["X-XSS-Protection"] = "0";
    context.Response.Headers["Content-Security-Policy"] = "default-src 'self'; script-src 'self'; connect-src 'self' https://localhost:5001 http://localhost:5000; object-src 'none'; frame-ancestors 'none'; base-uri 'self'";
    await next();
});

// Parameter pollution protection: reject duplicate query keys
app.Use(async (context, next) =>
{
    var duplicated = context.Request.Query
        .GroupBy(q => q.Key, StringComparer.OrdinalIgnoreCase)
        .Any(g => g.First().Value.Count > 1);
    if (duplicated)
    {
        context.Response.StatusCode = StatusCodes.Status400BadRequest;
        await context.Response.WriteAsync("Bad Request: duplicate query parameters detected.");
        return;
    }
    await next();
});

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

// CSRF validation for state-changing requests using token claim
app.Use(async (context, next) =>
{
    if (HttpMethods.IsPost(context.Request.Method)
        || HttpMethods.IsPut(context.Request.Method)
        || HttpMethods.IsDelete(context.Request.Method)
        || HttpMethods.IsPatch(context.Request.Method))
    {
        var endpoint = context.GetEndpoint();
        var allowsAnonymous = endpoint?.Metadata.GetMetadata<IAuthorizeData>() == null && endpoint?.Metadata.GetMetadata<IAllowAnonymous>() != null;

        if (!allowsAnonymous && context.User?.Identity?.IsAuthenticated == true)
        {
            var claimToken = context.User.FindFirst("csrf")?.Value;
            var headerToken = context.Request.Headers["X-CSRF-Token"].ToString();
            if (string.IsNullOrEmpty(claimToken) || string.IsNullOrEmpty(headerToken) || !string.Equals(claimToken, headerToken, StringComparison.Ordinal))
            {
                context.Response.StatusCode = StatusCodes.Status403Forbidden;
                await context.Response.WriteAsync("Forbidden: CSRF token missing or invalid.");
                return;
            }
        }
    }
    await next();
});

app.MapControllers();

// Do not expose default Identity endpoints in JWT-only API

// Seed dummy data...
// ================================================================================
using var scope = app.Services.CreateScope();
var services = scope.ServiceProvider;

try
{
    var context = services.GetRequiredService<AppDbContext>();
    var userManager = services.GetRequiredService<UserManager<User>>();
    var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
    await InitializeDb.SeedData(context, userManager);
    if (!await roleManager.RoleExistsAsync("Admin"))
    {
        await roleManager.CreateAsync(new IdentityRole("Admin"));
    }
    var admin = await userManager.FindByEmailAsync("michael@gmail.com");
    if (admin is not null && !await userManager.IsInRoleAsync(admin, "Admin"))
    {
        await userManager.AddToRoleAsync(admin, "Admin");
    }
}
catch (Exception ex)
{
    var logger = services.GetRequiredService<ILogger<Program>>();
    logger.LogError(ex, "Det gick fel vid migrering av databasen.");
}

app.Run();
