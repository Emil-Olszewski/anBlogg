using anBlogg.Domain.Services;
using anBlogg.Domain.Services.Implementations;
using Microsoft.Extensions.DependencyInjection;

namespace anBlogg.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddDomain(this IServiceCollection services)
        {
            services.AddTransient<ITagsInString, TagsInString>();
            return services;
        }
    }
}