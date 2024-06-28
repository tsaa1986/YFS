using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YFS.Core.Models;

namespace YFS.Core.Dtos
{
    public class OperationItemDto
    {
        public int Id { get; set; }
        public int OperationId { get; set; }
        public int CategoryId { get; set; }
        public string CategoryName { get; set; } = null!;
        public string? Description { get; set; }
        public int OperationCurrencyId { get; set; }
        public decimal CurrencyAmount { get; set; }
        public decimal OperationAmount { get; set; }
        public Category Category { get; set; } = null!;
    }
}
