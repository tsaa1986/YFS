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
            await FindByConditionAsync(m => m.RuleId.Equals(ruleId), false).Result.SingleOrDefaultAsync() 
            ?? throw new InvalidOperationException($"Rule not found");

        public async Task<IEnumerable<MonoSyncRule>> GetRules(int apiTokenId) => 
            await FindByConditionAsync(m => m.IsActive.Equals(true) && m.ApiTokenId.Equals(apiTokenId), false)
                .Result.OrderBy(m => m.Priority).ToListAsync();

        public async Task UpdateRule(MonoSyncRule updatedRule) => await UpdateAsync(updatedRule);
 
    }
}
