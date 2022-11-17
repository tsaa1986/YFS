using System.Linq.Expressions;

namespace YFS.Repo.GenericRepository.Interfaces
{
    public interface IRepositoryBase<T> where T: class
    {
        Task<IQueryable<T>> FindAllAsync(bool trackChanges);
        Task<IQueryable<T>> FindByConditionAsync(Expression<Func<T, bool>> expression, bool trackChanges);
        Task CreateAsync(T entity);
        Task UpdateAsync(T entity);
        Task RemoveAsync(T entity);
        /*
        IEnumerable<T> All { get; }
        Task<T> FindById(int id);
        Task<T> FindById(string id);*/

    }
}
