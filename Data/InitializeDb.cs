using api.Models;
using Microsoft.AspNetCore.Identity;

namespace api.Data;

public class InitializeDb
{
    public static async Task SeedData(AppDbContext context, UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
    {
        // Create roles if they don't exist
        if (!roleManager.Roles.Any())
        {
            var adminRole = new IdentityRole("Admin");
            var userRole = new IdentityRole("User");
            
            var adminResult = await roleManager.CreateAsync(adminRole);
            var userResult = await roleManager.CreateAsync(userRole);
            
            Console.WriteLine($"Admin role created: {adminResult.Succeeded}");
            Console.WriteLine($"User role created: {userResult.Succeeded}");
        }

        if (!userManager.Users.Any())
        {
            var users = new List<User>
            {
                new(){FirstName="Michael", LastName="Gustavsson", UserName="michael@gmail.com", Email="michael@gmail.com" },
                new(){FirstName="Eva", LastName="Eriksson", UserName="eva@gmail.com", Email="eva@gmail.com" },
                new(){FirstName="Charlotte", LastName="Magnusson", UserName="lotta@gmail.com", Email="lotta@gmail.com" },
            };

            foreach (var user in users)
            {
                var result = await userManager.CreateAsync(user, "Pa$$w0rd");
                if (result.Succeeded)
                {
                    // Assign Admin role to the first user (Michael)
                    if (user.Email == "michael@gmail.com")
                    {
                        var roleResult = await userManager.AddToRoleAsync(user, "Admin");
                        Console.WriteLine($"Admin role assigned to {user.Email}: {roleResult.Succeeded}");
                    }
                    else
                    {
                        var roleResult = await userManager.AddToRoleAsync(user, "User");
                        Console.WriteLine($"User role assigned to {user.Email}: {roleResult.Succeeded}");
                    }
                }
            }   
        }
    }
}
