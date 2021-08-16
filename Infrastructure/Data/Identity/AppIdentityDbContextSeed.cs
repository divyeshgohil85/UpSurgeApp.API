using Core.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data.Identity
{
    public class AppIdentityDbContextSeed
    {
        public static async Task SeedUsersAsync(UserManager<AppUser> userManager)
        {
            if (!userManager.Users.Any())
            {
                var user = new AppUser
                {
                    FirstName = "Admin",
                    Email = "jyoti.besturing@gmail.com",
                    UserName = "jyoti.besturing@gmail.com",
                };

                await userManager.CreateAsync(user, "Pa$$w0rd");
            }
        }
    }
}
