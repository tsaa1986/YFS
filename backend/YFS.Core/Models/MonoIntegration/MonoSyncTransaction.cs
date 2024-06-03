using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YFS.Core.Models.MonoIntegration
{
    public class MonoSyncTransaction
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string MonobankTransactionId { get; set; } = null!;

        [Required]
        public int OperationId { get; set; }

        public Operation Operation { get; set; } = null!;

        [Required]
        public DateTime SyncedOn { get; set; } = DateTime.UtcNow;
    }
}
