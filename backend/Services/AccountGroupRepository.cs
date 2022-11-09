﻿using System.Collections.Generic;
using System.Threading.Tasks;
using YFS.Data.Models;
using YFS.Data.Repository;

namespace YFS.Services
{
    public class AccountGroupRepository : RepositoryBase<AccountGroup>, IAccountGroupRepository
    {
        public AccountGroupRepository(RepositoryContext repositoryContext) : base(repositoryContext)
        {
           
        }

        public async Task CreateAccountGroup(AccountGroup ac) => await CreateAsync(ac);

        public async Task CreateAccountGroupsDefaultForUser(string userid) 
        {
            AccountGroup ac = new AccountGroup
            {
                UserId = userid,
                AccountGroupNameEn = "Cash",
                AccountGroupNameRu = "Наличные",
                AccountGroupNameUa = "Готівка"
            };
            
            await CreateAccountGroup(ac);
        }
      }
}
