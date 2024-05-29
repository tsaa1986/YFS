using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace YFS.Core.Models
{
    public class Category
    {
        [Key]
        public int Id { get; set; }

        public int RootId { get; set; }

        // If UserId is null, it's a default category
        [ForeignKey("AspNetUser")]
        public string? UserId { get; set; }

        [Column(TypeName = "VARCHAR")]
        [MaxLength(100)]
        public string? Note { get; set; }

        // Navigation property for translations
        public ICollection<CategoryTranslation> Translations { get; set; } = new List<CategoryTranslation>();
    }
}
