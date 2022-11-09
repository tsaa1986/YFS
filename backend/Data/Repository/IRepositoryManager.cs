using System.Threading.Tasks;

namespace YFS.Data.Repository
{
    public interface IRepositoryManager
    {
        IAccountGroupRepository AccountGroup { get; }
        IUserAuthenticationRepository UserAuthentication { get; }
        Task SaveAsync();
    }
}
