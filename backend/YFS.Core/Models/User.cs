using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace YFS.Core.Models
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
