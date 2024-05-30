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
    public class OperationItem
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public int OperationId { get; set; }
        [Required]
        public int CategoryId { get; set; }
        public Category Category { get; set; } = null!;
        [Required]
        [Column(TypeName = "decimal(10,2)")]
        public decimal CurrencyAmount { get; set; }
        [Required]
        [Column(TypeName = "decimal(10,2)")]
        public decimal OperationAmount { get; set; }
        [Column(TypeName = "VARCHAR")]
        [MaxLength(200, ErrorMessage = "Maximum length for the Description is 200.")]
        public string? Description { get; set; }  
    }
}
