namespace anBlogg.Application.Services.Models
{
    public interface IResourceParameters
    {
        string Fields { get; set; }
        string OrderBy { get; set; }
        int PageNumber { get; set; }
        int PageSize { get; set; }
    }
}