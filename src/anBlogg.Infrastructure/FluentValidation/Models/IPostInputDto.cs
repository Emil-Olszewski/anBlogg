namespace anBlogg.Infrastructure.FluentValidation.Models
{
    public interface IPostInputDto
    {
        string Contents { get; set; }
        string[] Tags { get; set; }
        string Title { get; set; }
    }
}