using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YFS.Core.Models
{
    public class OperationTag
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int OperationId { get; set; }
        public Operation Operation { get; set; } = new Operation();

        [Required]
        public int TagId { get; set; }
        public Tag Tag { get; set; } = new Tag();
    }
}
