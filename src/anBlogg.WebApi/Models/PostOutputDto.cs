using System;
using System.Collections.Generic;

namespace anBlogg.WebApi.Models
{
    public class PostOutputDto
    {
        public Guid Id { get; set; }
        public AuthorShortOutputDto Author { get; set; }
        public DateTime Created { get; set; }
        public DateTime Modified { get; set; }
        public string Title { get; set; }
        public string Contents { get; set; }
        public string[] Tags { get; set; }
        public int Score { get; set; }
        public List<CommentOutputDto> Comments { get; set; }
    }
}