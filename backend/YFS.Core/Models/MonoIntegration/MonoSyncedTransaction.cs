using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YFS.Core.Models.MonoIntegration
{
    public class MonoSyncedTransaction
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string MonoTransactionId { get; set; } = null!;

        [Required]
        public int OperationId { get; set; }

        public Operation Operation { get; set; } = null!;
        public int TransferOperationId { get; set; }
        [Required]
        public DateTime SyncedOn { get; set; } = DateTime.UtcNow;
    }
}
