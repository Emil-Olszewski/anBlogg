using anBlogg.Application.Services.Models;

namespace anBlogg.WebApi.ResourceParameters
{
    public class PostResourceParameters : IPostResourceParameters
    {
        public int PageNumber { get; set; }
        public int PostsDisplayed { get; set; }
        public string RequiredTags { get; set; }

        public PostResourceParameters()
        {
            PageNumber = 1;
            PostsDisplayed = Constants.DefaultPostsOnPage;
        }
    }
}
