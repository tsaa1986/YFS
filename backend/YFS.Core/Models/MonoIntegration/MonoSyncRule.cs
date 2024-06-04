using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YFS.Core.Models.MonoIntegration
{
    public class MonoSyncRule
    {
        [Key]
        public int RuleId { get; set; }

        [Required]
        [MaxLength(255)]
        public string RuleName { get; set; } = null!;

        [MaxLength(1000)]
        public string? Description { get; set; }

        [Required]
        public string Condition { get; set; } = null!; // JSON or specific format defining the condition

        [Required]
        public string Action { get; set; } = null!; // JSON or specific format defining the action

        public int Priority { get; set; } // Lower number indicates higher priority

        public bool IsActive { get; set; } = true;

        [Required]
        public DateTime CreatedOn { get; set; } = DateTime.UtcNow;

        [Required]
        [ForeignKey("ApiToken")]
        public int ApiTokenId { get; set; }
    }
}
