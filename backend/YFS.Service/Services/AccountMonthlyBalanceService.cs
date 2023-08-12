using AutoMapper;
using YFS.Core.Dtos;
using YFS.Service.Interfaces;

namespace YFS.Service.Services
{
    public class AccountMonthlyBalanceService : BaseService, IAccountMonthlyBalanceService
    {
        public AccountMonthlyBalanceService(IRepositoryManager repository, IMapper mapper) : base(repository, mapper)
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
                return ServiceResult<IEnumerable<AccountMonthlyBalanceDto>>.Error(ex.Message);
            }
        }
    }
}
