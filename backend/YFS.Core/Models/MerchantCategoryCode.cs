using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YFS.Core.Models
{
    public class MerchantCategoryCode
    {
        [Key]
        [Required]
        public int Code { get; set; }

        [Required]
        [MaxLength(255)]
        public string Description { get; set; } = null!;

        public string? Note { get; set; }

        [Required]
        public DateTime CreatedOn { get; set; } = DateTime.UtcNow;
    }
}
