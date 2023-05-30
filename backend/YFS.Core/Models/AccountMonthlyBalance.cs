using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace YFS.Core.Models
{
    public class AccountMonthlyBalance
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [ForeignKey("Account")]
        public int AccountId { get; set; }
        [Required]
        public DateTime StartDateOfMonth { get; set; }
        public int MonthNumber { get; set; }
        public int YearNumber { get; set; }
        [Required]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal MonthDebit { get; set; }
        [Required]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal MonthCredit { get; set; }
        [Required]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal OpeningMonthBalance { get; set; }
        [Required]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal ClosingMonthBalance { get; set; }

    }
}
