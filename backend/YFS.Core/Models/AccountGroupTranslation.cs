using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace YFS.Core.Models
{
    public class AccountGroupTranslation
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int AccountGroupId { get; set; }
        public AccountGroup AccountGroup { get; set; } = null!;

        [Required]
        [Column(TypeName = "VARCHAR")]
        [MaxLength(100, ErrorMessage = "Maximum length for the GroupName is 100.")]
        public string AccountGroupName { get; set; } = null!;

        [Required]
        [Column(TypeName = "VARCHAR")]
        [MaxLength(10, ErrorMessage = "Maximum length for the LanguageCode is 10.")]
        public string LanguageCode { get; set; } = null!; // Language code like "en", "ru", "ua"
    }
}
