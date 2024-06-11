using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YFS.Core.Models.MonoIntegration;

namespace YFS.Core.Models
{    public class Operation
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public int TransferOperationId { get; set; } //for transfer operation between accounts
        [Required]
        public string UserId { get; set; } = null!;
        [Required]
        public int TypeOperation { get; set; } //1-income,2-expense,3-transfer
        [Required]
        public int AccountId { get; set; }  //foreign key
        public Account Account { get; set; } = null!;  //nav propert
        [Required]
        public DateTime OperationDate { get; set; }
        public decimal TotalCurrencyAmount { get; set; }
        [Required]
        public int OperationCurrencyId { get; set; }
        public Currency? OperationCurrency { get; set; }
        [Column(TypeName = "decimal(10,2)")]
        public decimal ExchangeRate { get; set; }
        [Column(TypeName = "decimal(10,2)")]
        public decimal CashbackAmount { get; set; }
        [Required]
        [Column(TypeName = "decimal(10,2)")]
        public int MCC { get; set; }
        [Column(TypeName = "VARCHAR")]
        [MaxLength(200, ErrorMessage = "Maximum length for the Description is 200.")]
        public string? Description { get; set; }
        [Column(TypeName = "VARCHAR")]
        [MaxLength(200, ErrorMessage = "Maximum length for the GroupName is 200.")]
        public virtual List<OperationItem> OperationItems { get; set; } = new List<OperationItem>();
        public virtual ICollection<OperationTag> OperationTags { get; set; } = new List<OperationTag>();
        public MonoSyncedTransaction? MonoSyncedTransaction { get; set; }
    }
}
