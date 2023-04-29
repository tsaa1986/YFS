using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using YFS.Core.Models;

namespace YFS.Core.Dtos
{
    public class OperationDto
    {
        public int Id { get; set; }
        //public int TransferOperationId { get; set; } //for transfer operation between accounts
        //public string UserId { get; set; }
        public int CategoryId { get; set; }
        public int TypeOperation { get; set; } //1-income,2-expense,3-transfer
                                               //
        public int AccountId { get; set; }
        public string? AccountName { get; set; }
        public int OperationCurrencyId { get; set; }
        public decimal CurrencyAmount { get; set; }
        public decimal OperationAmount { get; set; }
        public DateTime OperationDate { get; set; }
        //public decimal ExchangeRate { get; set; }
        //public decimal CashbackAmount { get; set; }
        public decimal Balance { get; set; }
        //public int MCC { get; set; }
        public string? Description { get; set; }
        public string? Tag { get; set; }
    }
}
