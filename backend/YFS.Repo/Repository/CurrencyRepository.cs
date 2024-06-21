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
            => await FindAll(trackChanges).OrderBy(c => c.Name_en).ToListAsync();

        public async Task<Currency?> GetCurrencyByCodeAndCountry(int number, string country) => 
            await FindByCondition(c => c.Number == number && c.Country.Equals(country), false).SingleOrDefaultAsync();
    }
}
