using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmpAge.Models
{
    public class DBInit
    {
        public static async Task InitializeAsync(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            string adminEmail = "admin@mail.ru";
            string moderEmail = "moder@mail.ru";
            string applicEmail = "applic@mail.ru";
            string emploEmail = "emplo@mail.ru";
            string password = "Mnbvcxz1@";

            if (await roleManager.FindByNameAsync("applicant") == null)
            {
                await roleManager.CreateAsync(new IdentityRole("applicant"));
            }
            if (await roleManager.FindByNameAsync("employer") == null)
            {
                await roleManager.CreateAsync(new IdentityRole("employer"));
            }
            if (await roleManager.FindByNameAsync("admin") == null)
            {
                await roleManager.CreateAsync(new IdentityRole("admin"));
            }
            if (await roleManager.FindByNameAsync("moder") == null)
            {
                await roleManager.CreateAsync(new IdentityRole("moder"));
            }
            

            if (await userManager.FindByNameAsync(adminEmail) == null)
            {
                IdentityUser admin = new IdentityUser { Email = adminEmail, UserName = adminEmail };
                IdentityResult result = await userManager.CreateAsync(admin, password);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(admin, "admin");
                }
            }

            if (await userManager.FindByNameAsync(moderEmail) == null)
            {
                IdentityUser moder = new IdentityUser { Email = moderEmail, UserName = moderEmail };

                IdentityResult result = await userManager.CreateAsync(moder, password);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(moder, "moder");
                }
            }

            if (await userManager.FindByNameAsync(applicEmail) == null)
            {
                IdentityUser applic = new IdentityUser { Email = applicEmail, UserName = applicEmail };

                IdentityResult result = await userManager.CreateAsync(applic, password);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(applic, "applicant");
                }
            }

            if (await userManager.FindByNameAsync(emploEmail) == null)
            {
                IdentityUser emplo = new IdentityUser { Email = emploEmail, UserName = emploEmail };

                IdentityResult result = await userManager.CreateAsync(emplo, password);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(emplo, "applicant");
                }
            }
        }
    }
}
