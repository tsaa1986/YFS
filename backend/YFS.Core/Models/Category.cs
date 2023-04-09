using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace YFS.Core.Models
{
    public class Category
    {
        [Column("CategoryId")]
        [Key]
        public int Id { get; set; }
        public int RootId { get; set; }
        //if userid = null it's default category
        //[Required]
        [ForeignKey("AspNetUser")]
        public string? UserId { get; set; }

        [Column(TypeName = "VARCHAR")]
        [Required(ErrorMessage = "Category's name is a required field")]
        [MaxLength(100, ErrorMessage = "Maximum length for the Name is 100 characters.")]
        public string? Name_UA { get; set; }
        [Column(TypeName = "VARCHAR")]
        [Required(ErrorMessage = "Category's name is a required field")]
        [MaxLength(100, ErrorMessage = "Maximum length for the Name is 100 characters.")]
        public string? Name_ENG { get; set; }
        [Column(TypeName = "VARCHAR")]
        [Required(ErrorMessage = "Category's name is a required field")]
        [MaxLength(100, ErrorMessage = "Maximum length for the Name is 100 characters.")]
        public string? Name_RU { get; set; }

        [Column(TypeName = "VARCHAR")]
        [MaxLength(100)]
        public string? Note { get; set; }
    }
}
