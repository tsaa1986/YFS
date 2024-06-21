using Microsoft.EntityFrameworkCore;
using System.Security.Principal;
using YFS.Core.Models;
using YFS.Core.Models.MonoIntegration;
using YFS.Repo.Data;
using YFS.Repo.GenericRepository.Services;
using YFS.Service.Interfaces;

namespace YFS.Service.Services
{
    public class MonoSyncRuleRepository : RepositoryBase<MonoSyncRule>, IMonoSyncRuleRepository
    {
        public MonoSyncRuleRepository(RepositoryContext repositoryContext) : base(repositoryContext)
        {
        }

        public async Task AddRange(IEnumerable<MonoSyncRule> rules)
        {
            foreach (var rule in rules)
            {
                await CreateAsync(rule);
            }
        }

        public async Task AddRule(MonoSyncRule newRule) =>
            await CreateAsync(newRule);
        public async Task<MonoSyncRule?> GetRule(int ruleId) =>
            await FindByCondition(m => m.RuleId.Equals(ruleId), false)
                .SingleOrDefaultAsync() ?? throw new InvalidOperationException($"Rule not found");

        public async Task<IEnumerable<MonoSyncRule>> GetActiveRulesByApiTokenIdAsync(int apiTokenId) => 
            await FindByCondition(m => m.IsActive.Equals(true) && m.ApiTokenId.Equals(apiTokenId), false).ToListAsync();

        public async Task UpdateRule(MonoSyncRule updatedRule) => await UpdateAsync(updatedRule);
    }
}
