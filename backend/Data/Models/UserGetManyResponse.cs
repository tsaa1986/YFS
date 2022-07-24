using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace YFS.Data.Models
{
    public class UserGetManyResponse
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string UserEmail { get; set; }
        public DateTime Created { get; }
    }
}
