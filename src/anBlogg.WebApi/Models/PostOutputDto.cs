using anBlogg.Application.Services.Models;
using System;
using System.Collections.Generic;

namespace anBlogg.WebApi.Models
{
    public class PostOutputDto : IPostOutputDto
    {
        public Guid Id { get; set; }
        public IAuthorShortOutputDto Author { get; set; }
        public DateTime Created { get; set; }
        public DateTime Modified { get; set; }
        public string Title { get; set; }
        public string Contents { get; set; }
        public string[] Tags { get; set; }
        public int Score { get; set; }
        public List<ICommentOutputDto> Comments { get; set; }
    }
}