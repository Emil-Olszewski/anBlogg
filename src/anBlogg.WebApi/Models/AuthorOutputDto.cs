using System;

namespace anBlogg.WebApi.Models
{
    public class AuthorOutputDto
    {
        public Guid Id { get; set; }
        public string DisplayName { get; set; }
        public int Score { get; set; }
        public int PostsNumber { get; set; }
        public int CommentsNumber { get; set; }
    }
}
