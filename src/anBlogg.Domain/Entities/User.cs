using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace anBlogg.Domain.Entities
{
    public class User : IdentityUser
    {
        public IList<Post> Posts { get; set; }
        public IList<Comment> Comments { get; set; }
        public string AboutMe { get; set; }
        public string Localization { get; set; }
        public int Reputation { get; set; }

        public User()
        {
            Posts = new List<Post>();
            Comments = new List<Comment>();
        }
    }
}