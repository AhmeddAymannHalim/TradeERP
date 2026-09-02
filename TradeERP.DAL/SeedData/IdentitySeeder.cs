using Microsoft.AspNetCore.Identity;

namespace TradeERP.DAL.SeedData
{
    public static class IdentitySeeder
    {
        public const string AdminRole = "Admin";
        public const string EmployeeRole = "Employee";

        public static async Task SeedAsync(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            foreach (var role in new[] { AdminRole, EmployeeRole })
            {
                if (!await roleManager.RoleExistsAsync(role))
                    await roleManager.CreateAsync(new IdentityRole(role));
            }

            const string adminEmail = "admin@traderp.local";
            var adminUser = await userManager.FindByEmailAsync(adminEmail);
            if (adminUser == null)
            {
                adminUser = new IdentityUser
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    EmailConfirmed = true
                };

                var result = await userManager.CreateAsync(adminUser, "Admin@123");
                if (!result.Succeeded)
                    return;
            }

            if (!await userManager.IsInRoleAsync(adminUser, AdminRole))
                await userManager.AddToRoleAsync(adminUser, AdminRole);
        }
    }
}
