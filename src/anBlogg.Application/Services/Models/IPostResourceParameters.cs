namespace anBlogg.Application.Services.Models
{
    public interface IPostResourceParameters
    {
        int PageNumber { get; set; }
        int PostsDisplayed { get; set; }
        string RequiredTags { get; set; }
    }
}