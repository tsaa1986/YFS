using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using YFS.Core.Models;

namespace YFS.Core.Dtos
{
    public class AccountMonthlyBalanceDto
    {
        public int Id { get; set; }
        public int AccountId { get; set; }
        public DateTime StartDateOfMonth { get; set; }
        public int MonthNumber { get; set; }
        public int YearNumber { get; set; }
        public decimal MonthDebit { get; set; }
        public decimal MonthCredit { get; set; }
        public decimal OpeningMonthBalance { get; set; }
        public decimal ClosingMonthBalance { get; set; }
    }
}
