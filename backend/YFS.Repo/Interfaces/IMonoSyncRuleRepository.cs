using YFS.Core.Models;
using YFS.Core.Models.MonoIntegration;

namespace YFS.Service.Interfaces
{
    public interface IMonoSyncRuleRepository
    {
        Task AddRule(MonoSyncRule newRule);
        Task AddRange(IEnumerable<MonoSyncRule> rules);
        Task UpdateRule(MonoSyncRule updatedRule);
        Task<MonoSyncRule?> GetRule(int ruleId);
        Task<IEnumerable<MonoSyncRule>> GetActiveRulesByApiTokenIdAsync(int apiTokenId);
    }
}
