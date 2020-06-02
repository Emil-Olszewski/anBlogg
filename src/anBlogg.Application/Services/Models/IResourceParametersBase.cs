namespace anBlogg.Application.Services.Models
{
    public interface IResourceParametersBase
    {
        string Fields { get; set; }
        string OrderBy { get; set; }
        int PageNumber { get; set; }
        int PageSize { get; set; }
    }
}