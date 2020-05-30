namespace anBlogg.Application.Services.Models
{
    public interface IPostResourceParameters
    {
        string Tags { get; set; }
        string OrderBy { get; set; }
        int PageNumber { get; set; }
        int PageSize { get; set; }
    }
}