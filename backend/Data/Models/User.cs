using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace YFS.Data.Models
{
    public class User : IdentityUser
    {
        [Required]
        public DateTime CreatedOn { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }

        public List<AccountGroup> AccountsGroup { get; set; }
    }
}
