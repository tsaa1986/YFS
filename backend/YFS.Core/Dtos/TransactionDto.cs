using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace YFS.Core.Dtos
{
    public class TransactionDto
    {
        public int Id { get; set; }
        public string? UserId { get; set; }
        public int TypeTransaction { get; set; } //1-income,2-expense,3-transfer
        public int CategoryId { get; set; }
        public int? WithdrawFromAccountId { get; set; }
        public int? TargetAccountId { get; set; }
        public decimal? Amount { get; set; }
        public DateTime TransactionDate { get; set; }
        public string? Note { get; set; }
        public string? Tag { get; set; }
    }
}
