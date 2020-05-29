using System;

namespace anBlogg.WebApi.Models
{
    public class CommentOutputDto
    {
        public Guid Id { get; set; }
        public AuthorShortOutputDto Author { get; set; }
        public DateTime Created { get; set; }
        public string Contents { get; set; }
        public int Score { get; set; }
    }
}