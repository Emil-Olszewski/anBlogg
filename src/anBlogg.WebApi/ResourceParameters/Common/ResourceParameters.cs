using anBlogg.Application.Services.Models;

namespace anBlogg.WebApi.ResourceParameters.Common
{
    public abstract class ResourceParameters : IResourceParameters
    {
        protected readonly int maxPageSize = Constants.MaxPageSize;
        protected int pageSize = Constants.DefaultPageSize;

        public string Fields { get; set; }
        public string OrderBy { get; set; }
        public int PageNumber { get; set; } = Constants.DefaultPageNumber;

        public int PageSize
        {
            get => pageSize;
            set => pageSize = (value > maxPageSize) ? maxPageSize : value;
        }
    }
}