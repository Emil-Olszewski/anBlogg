using anBlogg.Infrastructure.FluentValidation.Models;

namespace anBlogg.WebApi.Models
{
    public class PostInputDto : IPostInputDto
    {
        public string Title { get; set; }
        public string Contents { get; set; }
        public string[] Tags { get; set; }
    }
}