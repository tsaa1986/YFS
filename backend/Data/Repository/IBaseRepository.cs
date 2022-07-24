using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace YFS.Data.Repository
{
    public interface IBaseRepository<T> where T: class
    {
        Task<T> Get(int id);
        Task<T> Add(T entity);
        Task<T> Update(T entity);
        Task<T> Delete(int id);
    }
}
