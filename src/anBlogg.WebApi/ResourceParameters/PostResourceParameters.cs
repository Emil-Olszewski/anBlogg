using anBlogg.Application.Services.Models;

namespace anBlogg.WebApi.ResourceParameters
{
    public class PostResourceParameters : IPostResourceParameters
    {
        private readonly int maxPageSize = Constants.MaxPageSize;
        private int pageSize = Constants.DefaultPageSize;

        public string Tags { get; set; }
        public string OrderBy { get; set; } = Constants.DefaultPostsOrdering;
        public int PageNumber { get; set; } = Constants.DefaultPageNumber;
        public int PageSize
        {
            get => pageSize;
            set => pageSize = (value > maxPageSize) ? maxPageSize : value;
        }

    }
}