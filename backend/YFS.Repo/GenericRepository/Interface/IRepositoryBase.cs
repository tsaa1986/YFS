using System.Linq.Expressions;

namespace YFS.Repo.GenericRepository.Interfaces
{
    public interface IRepositoryBase<T> where T: class
    {
        IQueryable<T> FindAll(bool trackChanges);
        IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression, bool trackChanges);
        //Task<T> GetOwnerByIdAsync(int _id);
        Task CreateAsync(T entity);
        Task UpdateAsync(T entity);
        Task RemoveAsync(T entity);
        Task SaveAsync();
        /*
        IEnumerable<T> All { get; }
        Task<T> FindById(int id);
        Task<T> FindById(string id);*/

    }
}
