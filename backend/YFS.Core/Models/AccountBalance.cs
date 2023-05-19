using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YFS.Core.Models
{
    public class AccountBalance
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [ForeignKey("Account")]
        public int AccountId { get; set; }
        public virtual Account? Account { get; set; }
        [Required]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal Balance { get; set; }
        public DateTime LastUpdateTime { get; set; }

    }
}
