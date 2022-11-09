using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace YFS.Data.Models
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
