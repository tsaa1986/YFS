using Microsoft.AspNetCore.Identity;
using System;

namespace YFS.Data.Models
{
    public class User : IdentityUser
    {
        //public DateTime Created { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
    }
}
