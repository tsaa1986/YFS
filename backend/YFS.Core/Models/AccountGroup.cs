using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace YFS.Core.Models
{
    public class AccountGroup
    {
        [Key]
        public int AccountGroupId { get; set; }

        [Required]
        [ForeignKey("AspNetUser")]
        public string UserId { get; set; } = null!;
        public User? User { get; set; }

        [Required]
        public int GroupOrderBy { get; set; }

        public ICollection<Account> Accounts { get; set; } = new List<Account>();

        // Navigation property for translations
        public ICollection<AccountGroupTranslation> Translations { get; set; } = new List<AccountGroupTranslation>();
    }
}
