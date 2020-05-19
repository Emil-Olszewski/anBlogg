using System;

namespace anBlogg.WebApi.Models
{
    public class PostOutputDto
    {
        public Guid Id { get; set; }
        public Guid AuthorId { get; set; }
        public DateTime Created { get; set; }
        public DateTime Modified { get; set; }
        public string Title { get; set; }
        public string Contents { get; set; }
        public string[] Tags { get; set; }
        public int Score { get; set; }
        public int Comments { get; set; }
    }
}
