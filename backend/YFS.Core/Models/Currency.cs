using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace YFS.Core.Models
{
    public class Currency
    {
        public int CurrencyId { get; set; }

        [Column(TypeName = "VARCHAR")]
        [MaxLength(10, ErrorMessage = "Maximum length for the GroupName is 10.")]
        public string? ShortNameUs { get; set; }

        [Column(TypeName = "VARCHAR")]
        [MaxLength(100, ErrorMessage = "Maximum length for the GroupName is 100.")]
        [Required]
        public string? Name_ru { get; set; }

        [Column(TypeName = "VARCHAR")]
        [MaxLength(100, ErrorMessage = "Maximum length for the GroupName is 100.")]
        public string? Name_ua { get; set; }

        [Column(TypeName = "VARCHAR")]
        [MaxLength(100, ErrorMessage = "Maximum length for the GroupName is 100.")]
        public string? Name_en { get; set; }

        public ICollection<Account> Accounts { get; set; }
    }
}
