using anBlogg.Infrastructure.FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace anBlogg.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {
            services.AddTransient<IValidator, Validator>();
            return services;
        }
    }
}