using Microsoft.EntityFrameworkCore;
using System;
using YFS.Core.Models;
using YFS.Repo.Data;
using YFS.Repo.GenericRepository.Services;
using YFS.Service.Interfaces;

namespace YFS.Service.Services
{
    public class CurrencyRepository : RepositoryBase<Currency>, ICurrencyRepository
    {
        public CurrencyRepository(RepositoryContext repositoryContext) : base(repositoryContext)
        {
        }
        public async Task<IEnumerable<Currency>> GetCurrencies(bool trackChanges)
            => await FindAllAsync(trackChanges).Result.OrderBy(c => c.Name_en).ToListAsync();
    }
}
