using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace YFS.Core.Models
{
    public static class IdentitySeedData
    {
        private const string adminUser = "Admin";
        private const string adminPassword = "Secret123$";
        
        public static async void EnsurePopulated(IApplicationBuilder app)
        {       
            /*UserManager<User> userManager = app.ApplicationServices
            .GetRequiredService<UserManager<User>>();
            IdentityUser user = await userManager.FindByIdAsync(adminUser);
            if (user == null)
            {
                user = new User();
                user.UserName = adminUser;
                user.
                await userManager.CreateAsync(user, adminPassword);
            }*/
        }
    }
}
