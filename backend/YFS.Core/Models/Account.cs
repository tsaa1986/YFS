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
        [ForeignKey("AspNetUser")]
        public string UserId { get; set; }
        [Column("Favorites")]
        public int Favorites { get; set; }

        [ForeignKey(nameof(AccountGroup))]
        public int AccountGroupId { get; set; }

        [ForeignKey(nameof(Currency))]
        public int CurrencyId { get; set; }
        public int BankId { get; set; }

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
    }
}
