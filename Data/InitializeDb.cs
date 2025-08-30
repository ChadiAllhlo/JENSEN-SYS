using api.Models;
using Microsoft.AspNetCore.Identity;

namespace api.Data;

public class InitializeDb
{
    public static async Task SeedData(AppDbContext context, UserManager<User>userManager)
    {
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
                await userManager.CreateAsync(user, "Pa$$w0rd");
            }   
        }
    }
}
