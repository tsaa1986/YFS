namespace YFS.Service.Interfaces
{
    public interface IRepositoryManager
    {
        IAccountGroupRepository AccountGroup { get; }
        IUserAuthenticationRepository UserAuthentication { get; }
        Task SaveAsync();
    }
}
