using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YFS.Core.Models
{
    public class AccountType
    {        
        [Key]
        public int AccountTypeId { get; set; }

        [Column(TypeName = "VARCHAR")]
        [Required(ErrorMessage = "AccountType's name is a required field")]
        [MaxLength(30, ErrorMessage = "Maximum length for the Name is 30 characters.")]
        public string? NameUa { get; set; }

        [Column(TypeName = "VARCHAR")]
        [Required(ErrorMessage = "AccountType's name is a required field")]
        [MaxLength(30, ErrorMessage = "Maximum length for the Name is 30 characters.")]
        public string? NameRu { get; set; }

        [Column(TypeName = "VARCHAR")]
        [Required(ErrorMessage = "AccountType's name is a required field")]
        [MaxLength(30, ErrorMessage = "Maximum length for the Name is 30 characters.")]
        public string? NameEn { get; set; }

        [Column(TypeName = "VARCHAR")]
        [MaxLength(255, ErrorMessage = "Maximum length for the Name is 255 characters.")]
        public string? NoteUa { get; set; }
        [Column(TypeName = "VARCHAR")]
        [MaxLength(255, ErrorMessage = "Maximum length for the Name is 255 characters.")]

        public string? NoteRu { get; set; }
        [Column(TypeName = "VARCHAR")]
        [MaxLength(255, ErrorMessage = "Maximum length for the Name is 255 characters.")]
        public string? NoteEn { get; set; }

        [Required]
        public int TypeOrderBy { get; set; }

        [Required]
        public DateTime CreatedOn { get; set; } = DateTime.UtcNow;

        public ICollection<Account> Accounts { get; set; } = new List<Account>();
    }
}
