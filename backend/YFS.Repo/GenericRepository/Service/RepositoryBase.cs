using YFS.Repo.GenericRepository.Interfaces;
using YFS.Repo.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace YFS.Repo.GenericRepository.Services;

public abstract class RepositoryBase<T> : IRepositoryBase<T> where T : class
{
    protected RepositoryContext RepositoryContext;
    public RepositoryBase(RepositoryContext repositoryContext) =>
        RepositoryContext = repositoryContext;
    public IQueryable<T> FindAll(bool trackChanges) =>
        !trackChanges ? RepositoryContext.Set<T>().AsNoTracking() : RepositoryContext.Set<T>();
    public IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression, bool trackChanges) =>
    !trackChanges ? RepositoryContext.Set<T>().Where(expression).AsNoTracking() : RepositoryContext.Set<T>().Where(expression);
    public async Task CreateAsync(T entity) => await Task.Run(() => RepositoryContext.Set<T>().Add(entity));
    //public async Task UpdateAsync(T entity) => await Task.Run(() => RepositoryContext.Set<T>().Update(entity));
    public async Task UpdateAsync(T entity)
    {
        var keyProperty = GetKeyProperty();
        if (keyProperty != null)
        {
            var entityId = keyProperty.GetValue(entity);
            var existingEntity = RepositoryContext.Set<T>().Local.FirstOrDefault(e => keyProperty.GetValue(e).Equals(entityId));
            if (existingEntity != null)
            {
                RepositoryContext.Entry(existingEntity).State = EntityState.Detached;
            }
        }

        RepositoryContext.Set<T>().Update(entity);
        await Task.CompletedTask;
    }
    public async Task RemoveAsync(T entity)
    {
        RepositoryContext.Set<T>().Remove(entity);
        await Task.CompletedTask;
    }
    private PropertyInfo GetKeyProperty()
    {
        var entityType = typeof(T);
        var keyProperty = entityType.GetProperties().FirstOrDefault(p => p.GetCustomAttributes<KeyAttribute>().Any());

        if (keyProperty == null)
        {
            keyProperty = entityType.GetProperty("Id");
        }

        return keyProperty;
    }
    //public async Task RemoveAsync(T entity) => await Task.Run(() => RepositoryContext.Set<T>().Remove(entity));
}
