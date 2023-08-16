using AutoMapper;
using Microsoft.Extensions.Logging;
using YFS.Core.Dtos;
using YFS.Service.Interfaces;

namespace YFS.Service.Services
{
    public class AccountMonthlyBalanceService : BaseService, IAccountMonthlyBalanceService
    {
        public AccountMonthlyBalanceService(IRepositoryManager repository, IMapper mapper, ILogger<BaseService> logger) : base(repository, mapper, logger)
        {
        }

        public async Task<ServiceResult<IEnumerable<AccountMonthlyBalanceDto>>> GetAccountMonthlyBalanceByAccountId(int accountId)
        {
            try
            {
                var balances = await _repository.AccountMonthlyBalance.GetAccountMonthlyBalanceByAccountId(accountId, false);
                var accountMonthlyBalancesDto = _mapper.Map<IEnumerable<AccountMonthlyBalanceDto>>(balances);
                return ServiceResult<IEnumerable<AccountMonthlyBalanceDto>>.Success(accountMonthlyBalancesDto);
            }
            catch(Exception ex)
            {
                _logger.LogError($"Something went wrong in the {nameof(GetAccountMonthlyBalanceByAccountId)} action {ex}");
                return ServiceResult<IEnumerable<AccountMonthlyBalanceDto>>.Error(ex.Message);
            }
        }
    }
}
