using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace YFS.Core.Models
{
    public class Transaction
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [ForeignKey("AspNetUser")]
        public string? UserId { get; set; }
        [Required]
        [ForeignKey("Category")]
        public int CategoryId { get; set; }
        [Required]
        public int TypeTransaction { get; set; } //1-income,2-expense,3-transfer        
        [ForeignKey("Account")]
        public int? WithdrawFromAccountId { get; set; }
        [ForeignKey("Account")]
        public int? TargetAccountId { get; set; }
        [Required]
        public decimal? Amount { get; set; }
        public DateTime TransactionDate { get; set; }
        [Column(TypeName = "VARCHAR")]
        [MaxLength(200, ErrorMessage = "Maximum length for the Name is 200 characters.")]
        public string? Note { get; set; }
        [Column(TypeName = "VARCHAR")]
        [MaxLength(200, ErrorMessage = "Maximum length for the GroupName is 200.")]
        public string? Tag { get; set; }        
    }
}
