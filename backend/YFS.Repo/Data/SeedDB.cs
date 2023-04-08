using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;
using YFS.Core.Models;

namespace YFS.Repo.Data
{
    public static class SeedDb
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            const string demoEmail = "demo@demo.com";
            const string demoPassword = "123$qweR";
            const string demoUsername = "Demo";
            const string demoFirst = "Demo";
            const string demoLast = "Account";

            RepositoryContext context =
            serviceProvider.GetRequiredService<RepositoryContext>();
            context.Database.EnsureCreated();

            var userManager = serviceProvider.GetRequiredService<UserManager<User>>();
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            // initially create user(s)
            if (!context.Users.Any())
            {
                User user = new User()
                {
                    Email = demoEmail,
                    SecurityStamp = Guid.NewGuid().ToString(),
                    UserName = demoUsername,
                    FirstName = demoFirst,
                    LastName = demoLast,
                    CreatedOn = DateTime.UtcNow,
                };
                var result = userManager.CreateAsync(user, demoPassword).Result;
            }

            // make sure we have some roles
            if (!context.Roles.Any())
            {
                if (roleManager.RoleExistsAsync(UserRoles.Admin) != null)
                    roleManager.CreateAsync(new IdentityRole(UserRoles.Admin));
                context.SaveChanges();
                if (roleManager.RoleExistsAsync(UserRoles.User) != null)
                    roleManager.CreateAsync(new IdentityRole(UserRoles.User));
                context.SaveChanges();
            }

            // get the demo user we just made
            var demoUser = userManager.FindByEmailAsync(demoEmail).Result;

            if (demoUser == null) { 
                return; }
              else { 
                userManager.AddToRoleAsync(demoUser, UserRoles.Admin);
                context.SaveChanges();
            }
            
            if (demoUser != null)
            {
                AccountGroup acGroup = new AccountGroup
                {
                    UserId = demoUser.Id,
                    AccountGroupNameEn = "Cash",
                    AccountGroupNameRu = "Наличные",
                    AccountGroupNameUa = "Готівка"
                };
                //create default group
                context.AccountGroups.AddAsync(acGroup);
                context.SaveChanges();
            }


            //context.Database.Migrate();
            //if (!context.Products.Any())
            //{
            //    context.Products.AddRange(
            // ...statements omiited for brevity...
            //    );
            //    context.SaveChanges();
            //}
        }
    }
}
