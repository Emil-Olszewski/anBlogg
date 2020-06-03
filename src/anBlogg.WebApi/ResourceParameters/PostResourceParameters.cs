using anBlogg.Application.Services.Models;

namespace anBlogg.WebApi.ResourceParameters
{
    public class PostResourceParameters : Common.ResourceParameters, IPostResourceParameters
    {
        public string Tags { get; set; }

        public PostResourceParameters()
        {
            OrderBy = Constants.DefaultPostsOrdering;
        }
    }
}