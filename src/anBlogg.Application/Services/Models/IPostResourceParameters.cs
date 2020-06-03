namespace anBlogg.Application.Services.Models
{
    public interface IPostResourceParameters : IResourceParameters
    {
        string Tags { get; set; }
    }
}