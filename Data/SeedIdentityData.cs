using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace LibrarySystem.Data;

public static class SeedIdentityData
{
    public static void EnsurePopulated(IApplicationBuilder app)
    {
        using var scope = app.ApplicationServices.CreateScope();
        var services = scope.ServiceProvider;

        var identityContext = services.GetRequiredService<LibraryIdentityDbContext>();
        identityContext.Database.EnsureCreated();

        var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
        var userManager = services.GetRequiredService<UserManager<IdentityUser>>();

        EnsureRoles(roleManager).GetAwaiter().GetResult();
        EnsureAdminUser(userManager).GetAwaiter().GetResult();
    }

    private static async Task EnsureRoles(RoleManager<IdentityRole> roleManager)
    {
        var roles = new[] { "Admin", "Member" };

        foreach (var role in roles)
        {
            if (!await roleManager.RoleExistsAsync(role))
            {
                await roleManager.CreateAsync(new IdentityRole(role));
            }
        }
    }

    private static async Task EnsureAdminUser(UserManager<IdentityUser> userManager)
    {
        // Demo amaçlı varsayılan admin hesabı.
        // Not: Gerçek projede bu bilgiler konfigürasyondan gelir ve şifre daha güçlü olur.
        const string adminUserName = "admin";
        const string adminEmail = "admin@library.local";
        const string adminPassword = "admin";

        var user = await userManager.FindByNameAsync(adminUserName);
        if (user == null)
        {
            // Admin kullanıcısı yoksa oluştur.
            user = new IdentityUser
            {
                UserName = adminUserName,
                Email = adminEmail,
                EmailConfirmed = true
            };

            var result = await userManager.CreateAsync(user, adminPassword);
            if (!result.Succeeded)
            {
                // Oluşturma başarısızsa sessizce çıkılır (seed işlemi uygulamayı kırmasın diye).
                return;
            }
        }

        // Admin rolü atanmadıysa ata.
        if (!await userManager.IsInRoleAsync(user, "Admin"))
        {
            await userManager.AddToRoleAsync(user, "Admin");
        }
    }
}
