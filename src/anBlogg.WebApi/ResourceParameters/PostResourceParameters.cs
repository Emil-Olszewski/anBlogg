using anBlogg.Application.Services.Models;
using anBlogg.WebApi.ResourceParameters.Common;

namespace anBlogg.WebApi.ResourceParameters
{
    public class PostResourceParameters : ResourceParametersBase, IPostResourceParameters
    {
        public string Tags { get; set; }

        public PostResourceParameters()
        {
            OrderBy = Constants.DefaultPostsOrdering;
        }
    }
}