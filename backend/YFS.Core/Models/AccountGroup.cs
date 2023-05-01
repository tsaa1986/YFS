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
        public string? UserId { get; set; }

        [Column(TypeName = "VARCHAR")]
        [MaxLength(100, ErrorMessage = "Maximum length for the GroupName is 100.")]
        [Required]
        public string? AccountGroupNameRu { get; set; }

        [Column(TypeName = "VARCHAR")]
        [MaxLength(100, ErrorMessage = "Maximum length for the GroupName is 100.")]
        [Required]
        public string? AccountGroupNameEn { get; set; }

        [Column(TypeName = "VARCHAR")]
        [MaxLength(100, ErrorMessage = "Maximum length for the GroupName is 100.")]
        [Required]
        public string? AccountGroupNameUa { get; set; }

        [Required]
        public int GroupOrderBy { get; set; }

        public ICollection<Account> Accounts { get; set; } = new List<Account>();       
    }
}
