namespace YFS.Service.Interfaces
{
    public interface IRepositoryManager
    {
        IAccountGroupRepository AccountGroup { get; }
        IAccountTypeRepository AccountType { get; }
        IAccountRepository Account { get; }
        ICurrencyRepository Currency { get; }
        ICategoryRepository Category { get; }
        IUserAuthenticationRepository UserAuthentication { get; }
        ITransactionRepository Transaction { get; }
        Task SaveAsync();
    }
}
