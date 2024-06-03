using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YFS.Core.Models
{
    public class AccountSyncSettings
    {
        public int Id { get; set; }
        public int AccountId { get; set; }
        public DateTime? FromSyncDate { get; set; }
        public DateTime? LastSuccessSyncDate { get; set; }
        public string? AdditionalSettings { get; set; }
    }
}
