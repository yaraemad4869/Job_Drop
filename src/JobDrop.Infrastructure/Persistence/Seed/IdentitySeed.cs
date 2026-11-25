using JobDrop.Domain.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace JobDrop.Infrastructure.Persistence.Seed;

public static class IdentitySeed
{
    public static async Task SeedAsync(IServiceProvider serviceProvider)
    {
        // Use ILoggerFactory to avoid CS0718 (static types cannot be used as type arguments)
        var loggerFactory = serviceProvider.GetRequiredService<ILoggerFactory>();
        var logger = loggerFactory.CreateLogger("IdentitySeed");
        
        var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
        var userManager = serviceProvider.GetRequiredService<UserManager<AppUser>>();

        string[] roleNames = { "Admin", "Company", "JobSeeker" };

        foreach (var roleName in roleNames)
        {
            if (!await roleManager.RoleExistsAsync(roleName))
            {
                await roleManager.CreateAsync(new IdentityRole(roleName));
                logger.LogInformation("Role '{Role}' created.", roleName);
            }
        }

        // Seed default admin user
        var adminEmail = "admin@jobdrop.local";
        if (await userManager.FindByEmailAsync(adminEmail) == null)
        {
            var adminUser = new AppUser
            {
                UserName = adminEmail,
                Email = adminEmail,
                Handle = "admin",
                IsActive = true,
                EmailConfirmed = true
            };

            var result = await userManager.CreateAsync(adminUser, "Admin@12345");
            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(adminUser, "Admin");
                logger.LogInformation("Default admin user created.");
            }
        }
    }
}
