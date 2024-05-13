using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace YFS.Core.Models
{
    public class ApiToken
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; } = null!;

        [MaxLength(255)]
        public string? TokenType { get; set; }

        [Required]
        [MaxLength(255)]
        public string TokenValue { get ; set; } = null!;

        [MaxLength(255)]
        public string? Url { get; set; }

        [MaxLength(500)]
        public string? Note { get; set; }

    }
}
