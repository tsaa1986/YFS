using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YFS.Data.Repository;

namespace YFS.Data.Models
{
    public interface IAccountGroupRepository : IBaseRepository<AccountGroup>
    {
        public IEnumerable<AccountGroup> GetAllByUser(int userid);
    }
}
