using Microsoft.Extensions.Configuration;

namespace anBlogg.WebApi
{
    public static class Constants
    {
        private static IConfigurationRoot configurationRoot;

        static Constants()
        {
            BuildConfigurationRoot();
        }

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
    }
}