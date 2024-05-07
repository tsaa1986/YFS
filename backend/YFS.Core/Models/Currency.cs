using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace YFS.Core.Models
{
    public class Currency
    {
        [Key]
        //[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CurrencyId { get; set; }
        public int Number { get; set; }
        [Column(TypeName = "VARCHAR")]
        [MaxLength(3, ErrorMessage = "Maximum length for the currency code is 3.")]
        public string Code { get; set; } // ISO currency code

        [Column(TypeName = "VARCHAR")]
        [MaxLength(100, ErrorMessage = "Maximum length for the country name is 100.")]
        public string Country { get; set; }  // Country that uses the currency

        [Column(TypeName = "VARCHAR")]
        [MaxLength(100, ErrorMessage = "Maximum length for the currency name en is 100.")]
        [Required]
        public string? Name_en { get; set; }

        [Column(TypeName = "VARCHAR")]
        [MaxLength(100, ErrorMessage = "Maximum length for the currency name ru is 100.")]
        public string? Name_ru { get; set; }

        [Column(TypeName = "VARCHAR")]
        [MaxLength(100, ErrorMessage = "Maximum length for the currency name ua is 100.")]
        public string? Name_ua { get; set; }
        public string Symbol { get; set; }  // Currency symbol, e.g., $, €

        public ICollection<Account> Accounts { get; set; } = new HashSet<Account>();
        
        public Currency()
        {
            // It's good practice to initialize collections to prevent null reference exceptions
            Accounts = new HashSet<Account>();
        }
    }
}
