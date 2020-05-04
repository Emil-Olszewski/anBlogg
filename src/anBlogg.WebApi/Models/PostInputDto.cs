namespace anBlogg.WebApi.Models
{
    public class PostInputDto
    {
        public string Title { get; set; }
        public string Contents { get; set; }
        public string[] Tags { get; set; }
    }
}
