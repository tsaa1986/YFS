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
    public class CategoryTranslation
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int CategoryId { get; set; }

        [Required]
        [Column(TypeName = "VARCHAR")]
        [MaxLength(100, ErrorMessage = "Maximum length for the CategoryName is 100.")]
        public string Name { get; set; } = null!;

        [Required]
        [Column(TypeName = "VARCHAR")]
        [MaxLength(10, ErrorMessage = "Maximum length for the LanguageCode is 10.")]
        public string LanguageCode { get; set; } = null!; // Language code like "en", "ru", "ua"
    }
}
