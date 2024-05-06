using YFS.Repo.Interfaces;

namespace YFS.Service.Interfaces
{
    public interface IRepositoryManager
    {
        IAccountGroupRepository AccountGroup { get; }
        IAccountTypeRepository AccountType { get; }
        IAccountRepository Account { get; }
        IAccountBalanceRepository AccountBalance { get; }
        IAccountMonthlyBalanceRepository AccountMonthlyBalance { get; }
        ICurrencyRepository Currency { get; }
        ICategoryRepository Category { get; }
        IUserAuthenticationRepository UserAuthentication { get; }
        IOperationRepository Operation { get; }
        IBankRepository Bank { get; }
        IBankSyncHistoryRepository BankSyncHistory { get; }
        Task SaveAsync();
    }
}
