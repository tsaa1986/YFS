using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace YFS.Core.Models
{
    public class AccountGroup
    {
        [Key]
        public int AccountGroupId { get; set; }

        [ForeignKey("AspNetUser")]
        public string UserId { get; set; }

        [Column(TypeName = "VARCHAR")]
        [StringLength(100)]
        public string AccountGroupNameRu { get; set; }

        [Column(TypeName = "VARCHAR")]
        [StringLength(100)]
        [Required]
        public string AccountGroupNameEn { get; set; }

        [Column(TypeName = "VARCHAR")]
        [StringLength(100)]
        public string AccountGroupNameUa { get; set; }

        [Required]
        public int GroupOrederBy { get; set; }     
        
    }
}
