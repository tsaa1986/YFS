using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YFS.Core.Models.MonoIntegration;
using YFS.Service.Services;

namespace YFS.Service.Interfaces
{
    public interface IMonoSyncRulesService
    {
        Task<ServiceResult<IEnumerable<MonoSyncRule>>> GetActiveRulesByApiTokenIdAsync(int apiTokenId);
        Task<ServiceResult<MonoSyncRule>> AddRuleAsync(MonoSyncRule newRuleDto);
        Task<ServiceResult<IEnumerable<MonoSyncRule>>> AddRulesAsync(IEnumerable<MonoSyncRule> rulesDto);
        Task<ServiceResult<MonoSyncRule>> UpdateRuleAsync(MonoSyncRule updatedRuleDto);
        Task<ServiceResult<MonoSyncRule>> GetRuleAsync(int ruleId);
    }
}
