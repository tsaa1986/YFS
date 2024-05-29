using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using YFS.Core.Models;

namespace YFS.Core.Dtos
{
    public class OperationDto
    {
        public enum OperationType
        {
            Expense = 1,
            Income = 2,
            Transfer = 3
        }

        public int Id { get; set; }
        public int TransferOperationId { get; set; } // for transfer operation between accounts
        public int TypeOperation { get; set; } // 1-income, 2-expense, 3-transfer
        public int AccountId { get; set; }
        public string AccountName { get; set; } = null!;
        public decimal TotalCurrencyAmount { get; set; }
        public int OperationCurrencyId { get; set; }
        public DateTime OperationDate { get; set; }
        public decimal Balance { get; set; }
        public string? Description { get; set; }
        // Collection of OperationItems
        public List<OperationItemDto> OperationItems { get; set; } = new List<OperationItemDto>();
    }
}
