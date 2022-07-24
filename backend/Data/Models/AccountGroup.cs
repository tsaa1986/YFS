using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace YFS.Data.Models
{
    public class AccountGroup
    {
        public int AccountGroupId { get; set; }
        public string AccountGroupNameRu { get; set; }
        public string AccountGroupNameEn { get; set; }
        public string AccountGroupNameUa { get; set; }

        public int GroupOrederBy { get; set; }
        public int UserId { get; set; }
    }
}
