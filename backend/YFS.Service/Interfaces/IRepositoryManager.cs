namespace YFS.Service.Interfaces
{
    public interface IRepositoryManager
    {
        IAccountGroupRepository AccountGroup { get; }
        IAccountTypeRepository AccountType { get; }
        IAccountRepository Account { get; }
        ICurrencyRepository Currency { get; }
        IUserAuthenticationRepository UserAuthentication { get; }
        Task SaveAsync();
    }
}
