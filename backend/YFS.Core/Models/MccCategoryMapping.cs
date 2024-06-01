using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YFS.Core.Models
{
    public class MccCategoryMapping
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int MccCode { get; set; }

        [Required]
        public int CategoryId { get; set; }

        [MaxLength(255)]
        public string Description { get; set; } = null!; // Description indicating it's from Monobank
    }
}
