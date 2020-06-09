namespace anBlogg.Application.Services.Helpers
{
    public class PagesLinks
    {
        public string? Previous { get; private set; }
        public string? Next { get; private set; }
        public bool HasPrevious { get => !string.IsNullOrWhiteSpace(Previous); }
        public bool HasNext { get => !string.IsNullOrWhiteSpace(Next); }

        public PagesLinks(string? previous, string? next)
        {
            Previous = previous;
            Next = next;
        }
    }
}