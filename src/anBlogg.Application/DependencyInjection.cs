using anBlogg.Application.Services;
using anBlogg.Application.Services.Implementations;
using Microsoft.Extensions.DependencyInjection;

namespace anBlogg.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddTransient<IBlogRepository, BlogRepository>();
            services.AddTransient<IPropertyMappingService, PropertyMappingService>();
            return services;
        }
    }
}