using Microsoft.EntityFrameworkCore;
using System;
using YFS.Core.Models;
using YFS.Repo.Data;
using YFS.Repo.GenericRepository.Services;
using YFS.Service.Interfaces;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace YFS.Service.Services
{
    public class AccountTypeRepository : RepositoryBase<AccountType>, IAccountTypeRepository
    {
        public AccountTypeRepository(RepositoryContext repositoryContext) : base(repositoryContext)
        {
        }
        public async Task<IEnumerable<AccountType>> GetAllAccountTypes(bool trackChanges, string language) =>
            await FindAll(trackChanges)
                    .Include(at => at.Translations)
                    .OrderBy(at => at.Translations.FirstOrDefault(t => t.Language == language).Name)
                    .ToListAsync();
    }
}
