using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace YFS.Core.Models
{
    public class Account
    {
        [Column("AccountId")]
        public int Id { get; set; }

        [Required]
        public string UserId { get; set; }
        public User User { get; set; }
        [Required]
        public int AccountStatus { get; set; } //0-disabled,1-enabled

        [Column(TypeName = "VARCHAR")]
        [MaxLength(40, ErrorMessage = "Maximum length for the Name is 40 characters.")]
        public string ?IBAN { get; set; }
        public int Favorites { get; set; }
        public int AccountGroupId { get; set; }
        public int AccountTypeId { get; set; }

        [Required]
        public int CurrencyId { get; set; }
        public Currency? Currency { get; set; }
        public int BankId { get; set; }
        public Bank? Bank { get; set; }

        [Column(TypeName = "VARCHAR")]
        [Required(ErrorMessage = "Acount's name is a required field")]
        [MaxLength(255, ErrorMessage = "Maximum length for the Name is 255 characters.")]
        public string Name { get; set; }
        public DateTime OpeningDate { get; set; }

        [Column(TypeName = "VARCHAR")]
        [MaxLength(255, ErrorMessage = "Maximum length for the Name is 255 characters.")]
        public string? Note { get; set; }

        [Column(TypeName = "decimal(10, 2)")]
        public decimal Balance { get; set; }

        public List<Operation> Operations { get; set; } = new List<Operation>();
    }
}
