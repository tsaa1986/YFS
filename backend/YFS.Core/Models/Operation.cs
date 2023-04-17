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
    public class Operation
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public int TransferOperationId { get; set; } //for transfer operation between accounts
        [Required]
        [ForeignKey("AspNetUser")]
        public string UserId { get; set; }
        [Required]
        public int CategoryId { get; set; }
        //public Category Category { get; set; }

        [Required]
        public int TypeOperation { get; set; } //1-income,2-expense,3-transfer
        [Required]
        public int AccountId { get; set; }

        //[ForeignKey("Currency")]
        public int OperationCurrencyId { get; set; }

        [Required]
        [Column(TypeName = "decimal(10,2)")]
        public decimal CurrencyAmount { get; set; }

        [Required]
        [Column(TypeName = "decimal(10,2)")]
        public decimal OperationAmount { get; set; }

        [Required]
        public DateTime OperationDate { get; set; }
        [Column(TypeName = "decimal(10,2)")]
        public decimal ExchangeRate { get; set; }
        [Column(TypeName = "decimal(10,2)")]
        public decimal CashbackAmount { get; set; }
        [Required]
        [Column(TypeName = "decimal(10,2)")]
        public decimal Balance { get; set; }
        public int MCC { get; set; }

        [Column(TypeName = "VARCHAR")]
        [MaxLength(200, ErrorMessage = "Maximum length for the Description is 200.")]
        public string? Description { get; set; }
        [Column(TypeName = "VARCHAR")]
        [MaxLength(200, ErrorMessage = "Maximum length for the GroupName is 200.")]
        public string? Tag { get; set; }        
    }
}
