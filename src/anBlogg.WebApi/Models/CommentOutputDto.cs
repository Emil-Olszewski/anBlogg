using anBlogg.Application.Services.Models;
using System;

namespace anBlogg.WebApi.Models
{
    public class CommentOutputDto : ICommentOutputDto
    {
        public Guid Id { get; set; }
        public IAuthorShortOutputDto Author { get; set; }
        public DateTime Created { get; set; }
        public string Contents { get; set; }
        public int Score { get; set; }
    }
}