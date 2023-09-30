using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using YFS.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace YFS.Repo.Data
{
    public static class SeedDb
    {
        public static List<AccountGroup> InitializeAccountGroupsDefault(string _userid)
        {
            List<AccountGroup> accountGroups = new List<AccountGroup>();
            if (!string.IsNullOrEmpty(_userid))
            {
                accountGroups.Add(new AccountGroup
                { UserId = _userid, AccountGroupNameEn = "Cash", AccountGroupNameRu = "Наличные", AccountGroupNameUa = "Готівка" });
                accountGroups.Add(new AccountGroup
                { UserId = _userid, AccountGroupNameEn = "Bank accounts", AccountGroupNameRu = "Банковские счета", AccountGroupNameUa = "Банківські рахунки" });
                accountGroups.Add(new AccountGroup
                { UserId = _userid, AccountGroupNameEn = "Internet accounts", AccountGroupNameRu = "Интернет счета", AccountGroupNameUa = "Інтернет рахунки" });
            }
            return accountGroups;
        }
        public static List<Account> InitializeAccountsDefault(string _userid, List<AccountGroup> _accountGroups)
        {
            List<Account> accounts = new List<Account>();

            if(!string.IsNullOrEmpty(_userid) & (_accountGroups != null)) {
                foreach (AccountGroup accGroup in _accountGroups)
                {
                    if (accGroup.AccountGroupNameEn == "Cash") {
                         accounts.Add(new Account { UserId = _userid, AccountStatus=1, Favorites = 1, AccountGroupId = accGroup.AccountGroupId, AccountTypeId = 1,
                            CurrencyId = 980, BankId = 1, Name = "Wallet", OpeningDate = new DateTime(),
                            Note = "wallet uah", AccountBalance = new AccountBalance { Balance = 0 }
                         });
                        accounts.Add(new Account
                        {
                            UserId = _userid,
                            AccountStatus = 1,
                            Favorites = 0,
                            AccountGroupId = accGroup.AccountGroupId,
                            AccountTypeId = 1,
                            CurrencyId = 840,
                            BankId = 1,
                            Name = "Wallet",
                            OpeningDate = new DateTime(),
                            Note = "wallet usd",
                            AccountBalance = new AccountBalance()
                        });
                        accounts.Add(new Account
                        {
                            UserId = _userid,
                            AccountStatus = 1,
                            Favorites = 0,
                            AccountGroupId = accGroup.AccountGroupId,
                            AccountTypeId = 1,
                            CurrencyId = 978,
                            BankId = 1,
                            Name = "Wallet",
                            OpeningDate = new DateTime(),
                            Note = "wallet euro",
                            AccountBalance = new AccountBalance()
                        });
                    }
                    if (accGroup.AccountGroupNameEn == "Bank accounts")
                    {
                        accounts.Add(new Account
                        {
                            UserId = _userid,
                            Favorites = 0,
                            AccountStatus = 1,
                            AccountGroupId = accGroup.AccountGroupId,
                            AccountTypeId = 4,
                            CurrencyId = 840,
                            BankId = 1,
                            Name = "Mono",
                            OpeningDate = new DateTime(),
                            Note = "monobank usd",
                            AccountBalance = new AccountBalance()
                        });
                        accounts.Add(new Account
                        {
                            UserId = _userid,
                            AccountStatus = 1,
                            Favorites = 1,
                            AccountGroupId = accGroup.AccountGroupId,
                            AccountTypeId = 4,
                            CurrencyId = 980,
                            BankId = 1,
                            Name = "BlackMono",
                            OpeningDate = new DateTime(),
                            Note = "monobank uah",
                            AccountBalance = new AccountBalance()
                        });
                        accounts.Add(new Account
                        {
                            UserId = _userid,
                            AccountStatus = 1,
                            Favorites = 0,
                            AccountGroupId = accGroup.AccountGroupId,
                            AccountTypeId = 4,
                            CurrencyId = 980,
                            BankId = 1,
                            Name = "WhiteMono",
                            OpeningDate = new DateTime(),
                            Note = "monobank uah",
                            AccountBalance = new AccountBalance()
                        });
                        accounts.Add(new Account
                        {
                            UserId = _userid,
                            Favorites = 0,
                            AccountStatus = 1,
                            AccountGroupId = accGroup.AccountGroupId,
                            AccountTypeId = 4,
                            CurrencyId = 978,
                            BankId = 1,
                            Name = "Mono",
                            OpeningDate = new DateTime(),
                            Note = "monobank euro",
                            AccountBalance = new AccountBalance()
                        });
                    }
                }
            }

            return accounts;
        }
        public static async Task Initialize(IServiceProvider serviceProvider)
        {
            const string demoEmail = "demo@demo.com";
            const string demoPassword = "123$qweR";
            const string demoUsername = "Demo";
            const string demoFirst = "Demo";
            const string demoLast = "Account";

            using (var scope = serviceProvider.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<RepositoryContext>();
                await context.Database.EnsureCreatedAsync();

                var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();
                var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

                if (!await context.Users.AnyAsync())
                {
                    var user = new User()
                    {
                        Email = demoEmail,
                        SecurityStamp = Guid.NewGuid().ToString(),
                        UserName = demoUsername,
                        FirstName = demoFirst,
                        LastName = demoLast,
                        CreatedOn = DateTime.UtcNow,
                    };
                    var result = await userManager.CreateAsync(user, demoPassword);

                    if (!result.Succeeded)
                    {
                        // Handle user creation failure
                        // Log or throw an exception as needed
                    }
                }

                // Make sure we have some roles
                if (!await context.Roles.AnyAsync())
                {
                    // Create roles if they don't exist
                    if (!await roleManager.RoleExistsAsync(UserRoles.Admin))
                    {
                        await roleManager.CreateAsync(new IdentityRole(UserRoles.Admin));
                    }

                    if (!await roleManager.RoleExistsAsync(UserRoles.User))
                    {
                        await roleManager.CreateAsync(new IdentityRole(UserRoles.User));
                    }
                    await context.SaveChangesAsync();
                }

                // Get the demo user we just made
                var demoUser = await userManager.FindByEmailAsync(demoEmail);

                if (demoUser != null)
                {
                    if (!await userManager.IsInRoleAsync(demoUser, UserRoles.Admin))
                    {
                        await userManager.AddToRoleAsync(demoUser, UserRoles.Admin);
                    }

                    // Create default group and accounts here as needed
                    // Make sure to use asynchronous EF Core operations and await them
                    var accountGroupUser = context.AccountGroups.Where(a => a.UserId == demoUser.Id).ToList();
                    var accounts = context.Accounts.Where(a => a.UserId == demoUser.Id).ToList();

                    if (accountGroupUser.Count == 0)
                    {
                        foreach (AccountGroup agd in InitializeAccountGroupsDefault(demoUser.Id))
                        {
                            await context.AccountGroups.AddAsync(agd);
                        }
                        await context.SaveChangesAsync();
                        accountGroupUser = context.AccountGroups.Where(a => a.UserId == demoUser.Id).ToList();
                    }

                    //create default accounts
                    if (accounts.Count == 0)
                    {
                        List<Account> la = InitializeAccountsDefault(demoUser.Id, accountGroupUser);
                        foreach (Account account in la)
                        {
                            await context.Accounts.AddAsync(account);
                        }
                        await context.SaveChangesAsync();
                    }
                }
            }

        }
    }
}
