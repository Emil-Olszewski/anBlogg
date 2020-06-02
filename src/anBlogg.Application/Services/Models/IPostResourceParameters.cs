namespace anBlogg.Application.Services.Models
{
    public interface IPostResourceParameters : IResourceParametersBase
    {
        string Tags { get; set; }
    }
}