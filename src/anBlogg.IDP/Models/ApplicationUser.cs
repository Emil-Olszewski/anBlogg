using Microsoft.AspNetCore.Identity;
using System;

namespace anBlogg.IDP.Models
{
    public class ApplicationUser : IdentityUser
    {
        public Guid AuthorId { get; set; }
    }
}
