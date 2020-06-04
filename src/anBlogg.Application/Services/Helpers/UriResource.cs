using Microsoft.AspNetCore.Mvc;

namespace anBlogg.Application.Services.Helpers
{
    public class UriResource
    {
        public IUrlHelper UrlHelper { get; private set; }
        public string GetMethodName { get; private set; }

        public UriResource(IUrlHelper urlHelper, string getMethodName = "")
        {
            UrlHelper = urlHelper;
            GetMethodName = getMethodName;
        }
    }
}
