using EorDSU.Models;
using Microsoft.AspNetCore.Identity;

namespace EorDSU.DBService
{
    public class RoleInitializer
    {
        public static async Task InitializeAsync(string adminLogin, string password, UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
        {
            if (await roleManager.FindByNameAsync("admin") == null)
            {
                await roleManager.CreateAsync(new IdentityRole("admin"));
            }
            if (await userManager.FindByNameAsync(adminLogin) == null)
            {
                User admin = new User { UserName = adminLogin };
                IdentityResult result = await userManager.CreateAsync(admin, password);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(admin, "admin");
                }
            }
        }
    }
}
