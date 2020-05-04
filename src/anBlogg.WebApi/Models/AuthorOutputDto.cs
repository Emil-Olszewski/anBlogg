using System;

namespace anBlogg.WebApi.Models
{
    public class AuthorOutputDto
    {
        public Guid Id { get; set; }
        public string DisplayName { get; set; }
        public int NumberOfPosts { get; set; }
        public int NumberOfComments { get; set; }
        public int Score { get; set; }
    }
}
