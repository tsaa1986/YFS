using System.Threading.Tasks;

namespace YFS.Data.Repository
{
    public interface IRepositoryManager
    {
        IUserAuthenticationRepository UserAuthentication { get; }
        Task SaveAsync();
    }
}
