using Microsoft.Extensions.Configuration;

namespace anBlogg.Domain
{
    public static class Constants
    {
        private static IConfigurationRoot configurationRoot;

        static Constants() =>
            BuildConfigurationRoot();

        private static void BuildConfigurationRoot()
        {
            var configurationBuilder = new ConfigurationBuilder();
            configurationBuilder.AddJsonFile("web.config.json");
            configurationRoot = configurationBuilder.Build();
        }

        public static string DefaultPostsOrdering =>
            configurationRoot["WebConfig:defaultPostsOrdering"];

        public static int DefaultPageNumber =>
            int.Parse(configurationRoot["WebConfig:defaultPageNumber"]);

        public static int MaxPageSize =>
            int.Parse(configurationRoot["WebConfig:maxPageSize"]);

        public static int DefaultPageSize =>
            int.Parse(configurationRoot["WebConfig:defaultPageSize"]);

        public static string MediaTypeRoot =>
            configurationRoot["WebConfig:MediaTypes:root"];

        public static string MainMediaType =>
            MediaTypeRoot + configurationRoot["WebConfig:MediaTypes:main"];

        public static string AuthorFullMediaType =>
            MediaTypeRoot + configurationRoot["WebConfig:MediaTypes:author-full"];

        public static string AuthorFullHateoasMediaType =>
            MediaTypeRoot + configurationRoot["WebConfig:MediaTypes:author-full-hateoas"];

        public static string AuthorShortMediaType =>
            MediaTypeRoot + configurationRoot["WebConfig:MediaTypes:author-short"];

        public static string AuthorShortHateoasMediaType =>
            MediaTypeRoot + configurationRoot["WebConfig:MediaTypes:author-short-hateoas"];

        public static string[] MediaTypes =>
            new string[]
            {
                MediaTypeRoot, MainMediaType, AuthorFullMediaType,
                AuthorFullHateoasMediaType, AuthorShortMediaType,
                AuthorShortHateoasMediaType
            };
    }
}