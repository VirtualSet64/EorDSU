﻿using DomainServices.Entities;
using DomainServices.DtoModels.Account;
using Microsoft.AspNetCore.Identity;

namespace DomainServices.DBService
{
    public class RoleInitializer
    {
        public static async Task InitializeAsync(List<LoginViewModel> employees, UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
        {
            if (await roleManager.FindByNameAsync("methodist") == null)
            {
                await roleManager.CreateAsync(new IdentityRole("methodist"));
            }
            foreach (var employee in employees)
            {
                if (await roleManager.FindByNameAsync(employee.Login) == null)
                {
                    await roleManager.CreateAsync(new IdentityRole(employee.Login));
                }
                if (await userManager.FindByNameAsync(employee.Login) == null)
                {
                    User admin = new()
                    {
                        UserName = employee.Login,
                    };
                    if (employee.Login == "umu")
                        admin.Faculty = new List<UmuAndFaculty>() {
                            new UmuAndFaculty{
                                FacultyId = 1,
                                UserId = admin.Id
                            },
                            new UmuAndFaculty{
                                FacultyId = 2,
                                UserId = admin.Id
                            }
                        };
                    IdentityResult result = await userManager.CreateAsync(admin, employee.Password);
                    if (result.Succeeded)
                    {
                        await userManager.AddToRoleAsync(admin, employee.Login);
                    }
                }
            }
        }
    }
}
