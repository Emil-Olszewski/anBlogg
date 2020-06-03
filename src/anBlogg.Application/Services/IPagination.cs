using anBlogg.Application.Services.Helpers;
using anBlogg.Application.Services.Models;
using Microsoft.AspNetCore.Mvc;

namespace anBlogg.Application.Services
{
    public interface IPagination
    {
        Header CreateHeader<T>(PagedList<T> elements,
            IResourceParameters parameters, IUrlHelper url);
    }
}