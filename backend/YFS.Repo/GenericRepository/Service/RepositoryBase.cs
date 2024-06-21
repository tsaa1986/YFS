using YFS.Repo.GenericRepository.Interfaces;
using YFS.Repo.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace YFS.Repo.GenericRepository.Services;

public abstract class RepositoryBase<T> : IRepositoryBase<T> where T : class
{
    protected RepositoryContext RepositoryContext;

    public IQueryable<T> FindAll(bool trackChanges) =>
        !trackChanges ? RepositoryContext.Set<T>().AsNoTracking() : RepositoryContext.Set<T>();
    public RepositoryBase(RepositoryContext repositoryContext) =>   
        RepositoryContext = repositoryContext;
    public async Task CreateAsync(T entity) => await Task.Run(() => RepositoryContext.Set<T>().Add(entity));
    public async Task UpdateAsync(T entity) => await Task.Run(() => RepositoryContext.Set<T>().Update(entity));
    //public async Task UpdateAsync(T entity) => RepositoryContext.Set<T>().Update(entity);

    public async Task RemoveAsync(T entity) => await Task.Run(() => RepositoryContext.Set<T>().Remove(entity));
    public IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression, bool trackChanges) =>
        !trackChanges ? /*await Task.Run(() => */RepositoryContext.Set<T>().Where(expression).AsNoTracking() : /*await Task.Run(() => */RepositoryContext.Set<T>().Where(expression);
}
