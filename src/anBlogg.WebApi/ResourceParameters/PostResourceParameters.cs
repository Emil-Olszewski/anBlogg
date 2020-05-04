using anBlogg.Application.Services.Models;

namespace anBlogg.WebApi.ResourceParameters
{
    public class PostResourceParameters : IPostResourceParameters
    {
        public string RequiredTags { get; set; }
    }
}
