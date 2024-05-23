using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YFS.Core.Models
{
    public class AccountType
    {
        [Key]
        public int AccountTypeId { get; set; }
        public virtual ICollection<AccountTypeTranslation> Translations { get; set; } = new List<AccountTypeTranslation>();
        [Required]
        public bool IsEnabled { get; set; } = false; //default false
        [Required]
        public int TypeOrderBy { get; set; }

        [Required]
        public DateTime CreatedOn { get; set; } = DateTime.UtcNow;
        public ICollection<Account> Accounts { get; set; } = new List<Account>();
    }
}
