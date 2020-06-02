using anBlogg.Application.Services.Helpers;
using anBlogg.Application.Services.Models;
using Microsoft.AspNetCore.Mvc;

namespace anBlogg.Application.Services
{
    public interface IPagination
    {
        Header CreatePaginationHeader<T>(PagedList<T> elements, 
            IResourceParametersBase parameters, IUrlHelper url);
    }
}