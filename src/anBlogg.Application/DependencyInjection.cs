using anBlogg.Application.Services.Implementations;
using anBlogg.Domain.Services.Implementations;
using Microsoft.Extensions.DependencyInjection;

namespace anBlogg.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddTransient<Domain.Services.ITagsInString, TagsInString>();
            services.AddTransient<Services.IBlogRepository, BlogRepository>();
            return services;
        }
    }
}
