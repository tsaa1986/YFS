using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace YFS.Data.Repository
{
    public interface IRepositoryBase<T> where T: class
    {
        Task CreateAsync(T entity);
        Task UpdateAsync(T entity);
        Task RemoveAsync(T entity);
        /*
        IEnumerable<T> All { get; }
        Task<T> FindById(int id);
        Task<T> FindById(string id);*/

    }
}
