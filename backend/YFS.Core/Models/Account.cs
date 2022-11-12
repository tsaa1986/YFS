using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace YFS.Core.Models
{
    public class Account
    {
        [Column("AccountId")]
        public int Id { get; set; }

        [Column(TypeName = "VARCHAR")]
        [Required(ErrorMessage = "Acount's name is a required field")]
        [MaxLength(255, ErrorMessage = "Maximum length for the Name is 255 characters.")]
        public string? Name { get; set; }

        [ForeignKey(nameof(AccountGroup))]
        public int AccountGroupId { get; set; }
    }
}
