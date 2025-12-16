using Microsoft.AspNetCore.Identity;
using FlipkartClone.Constants;

namespace FlipkartClone.Data
{
    public static class DbSeeder
    {
        public static async Task SeedRolesAndAdminAsync(IServiceProvider service)
        {
            Console.WriteLine("--> [Seeder] Starting Seeding Process...");

            var userManager = service.GetService<UserManager<IdentityUser>>();
            var roleManager = service.GetService<RoleManager<IdentityRole>>();

            // 1. Create Roles
            if (!await roleManager.RoleExistsAsync(Roles.Admin))
            {
                await roleManager.CreateAsync(new IdentityRole(Roles.Admin));
                Console.WriteLine("--> [Seeder] Admin Role Created.");
            }
            
            if (!await roleManager.RoleExistsAsync(Roles.Customer))
            {
                await roleManager.CreateAsync(new IdentityRole(Roles.Customer));
                Console.WriteLine("--> [Seeder] Customer Role Created.");
            }

            // 2. Create Admin User
            var adminEmail = "admin@flipkart.com";
            var adminUser = await userManager.FindByEmailAsync(adminEmail);

            if (adminUser == null)
            {
                var user = new IdentityUser
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    EmailConfirmed = true
                };

                // NOTE: Password must have Upper, Lower, Digit, and Special Character
                var result = await userManager.CreateAsync(user, "Admin@123"); 
                
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, Roles.Admin);
                    Console.WriteLine("--> [Seeder] Admin User 'admin@flipkart.com' Created Successfully!");
                }
                else
                {
                    // THIS IS THE IMPORTANT PART: Print why it failed
                    Console.WriteLine("--> [Seeder] FAILED to create Admin User. Errors:");
                    foreach (var error in result.Errors)
                    {
                        Console.WriteLine($"    - {error.Description}");
                    }
                }
            }
            else
            {
                Console.WriteLine("--> [Seeder] Admin user already exists.");
            }
        }
    }
}