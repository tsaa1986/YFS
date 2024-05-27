using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YFS.Core.Models
{
    public class Tag
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string UserId { get; set; } = null!;

        [Required]
        [MaxLength(200, ErrorMessage = "Maximum length for the Tag is 200.")]
        public string Name { get; set; } = null!;

        public virtual ICollection<OperationTag> OperationTags { get; set; } = new List<OperationTag>();

    }
}
