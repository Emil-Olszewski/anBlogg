using anBlogg.Application.Services.Helpers;

namespace anBlogg.Application.Services
{
    public interface IPagination
    {
        Header CreateHeader<T>(PagedList<T> elements);

        PagesLinks CreatePagesLinks<T1, T2>(PagedList<T1> elements,
            T2 parameters, UriResource uriResource);

        string CreateResourceUri<T>
            (T source, UriResource uriResource, ResourceUriType type);
    }
}