using Microsoft.AspNetCore.Mvc;

namespace anBlogg.Application.Services.Helpers
{
    public class ResourceUriHelper
    {
        public IUrlHelper UrlHelper { get; private set; }
        public string GetMethodName { get; set; }

        public ResourceUriHelper(IUrlHelper urlHelper, string getMethodName = "")
        {
            UrlHelper = urlHelper;
            GetMethodName = getMethodName;
        }
    }
}