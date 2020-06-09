using anBlogg.Application.Services.Models;
using anBlogg.Domain;

namespace anBlogg.WebApi.ResourceParameters
{
    public class PostResourceParameters : BasicResourceParameters, IPostResourceParameters
    {
        public string Tags { get; set; }

        public PostResourceParameters() =>
            OrderBy = Constants.DefaultPostsOrdering;
    }
}