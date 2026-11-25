using JobDrop.Domain.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace JobDrop.Infrastructure.Persistence.Seed;

public static class IdentitySeed
{
    public static async Task SeedRolesAndAdminAsync(IServiceProvider services)
    {
        using var scope = services.CreateScope();
        var sp = scope.ServiceProvider;
        var logger = sp.GetRequiredService<ILogger<ApplicationDbContext>>();
        var roleManager = sp.GetRequiredService<RoleManager<AppRole>>();
        var userManager = sp.GetRequiredService<UserManager<AppUser>>();

        var roles = new[] { "JobSeeker", "CompanyOwner", "CompanyMember", "PlatformAdmin" };
        foreach (var role in roles)
        {
            if (!await roleManager.RoleExistsAsync(role))
            {
                var result = await roleManager.CreateAsync(new AppRole { Name = role, NormalizedName = role.ToUpperInvariant() });
                if (!result.Succeeded)
                    logger.LogWarning("Failed to create role {Role}: {Errors}", role, string.Join(',', result.Errors.Select(e => e.Description)));
            }
        }

        const string adminEmail = "admin@jobdrop.local";
        const string adminPassword = "Admin@12345678";
        if (await userManager.FindByEmailAsync(adminEmail) is null)
        {
            var admin = new AppUser
            {
                Email = adminEmail,
                UserName = adminEmail,
                Handle = "platform_admin",
                EmailConfirmed = true,
                LockoutEnabled = true
            };
            var result = await userManager.CreateAsync(admin, adminPassword);
            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(admin, "PlatformAdmin");
                logger.LogInformation("Seeded PlatformAdmin user: {Email}", adminEmail);
            }
        }
    }
}