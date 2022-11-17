namespace YFS.Service.Interfaces
{
    public interface IRepositoryManager
    {
        IAccountGroupRepository AccountGroup { get; }
        IAccountTypeRepository AccountType { get; }
        IUserAuthenticationRepository UserAuthentication { get; }
        Task SaveAsync();
    }
}
