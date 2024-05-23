using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YFS.Core.Models
{
    public class AccountTypeTranslation
    {
        [Key]
        public int AccountTypeTranslationId { get; set; }

        [ForeignKey("AccountType")]
        public int AccountTypeId { get; set; }
        public AccountType AccountType { get; set; } = null!;

        [Required]
        public string Language { get; set; } = null!;

        [Column(TypeName = "VARCHAR")]
        [Required(ErrorMessage = "AccountType's name is a required field")]
        [MaxLength(30, ErrorMessage = "Maximum length for the Name is 30 characters.")]
        public string Name { get; set; } = null!;

        [Column(TypeName = "VARCHAR")]
        [MaxLength(255, ErrorMessage = "Maximum length for the Note is 255 characters.")]
        public string? Description { get; set; }
    }
}
