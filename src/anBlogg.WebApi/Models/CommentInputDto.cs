using anBlogg.Infrastructure.FluentValidation.Models;

namespace anBlogg.WebApi.Models
{
    public class CommentInputDto : ICommentInputDto
    {
        public string AuthorId { get; set; }
        public string Contents { get; set; }
    }
}