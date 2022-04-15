using Microsoft.Extensions.DependencyInjection;
using WarehouseService.Domain.Abstractions;
using WarehouseService.Domain.Services;

namespace WarehouseService.Domain
{
    public static class DefaultDomainModule
    {
        public static IServiceCollection RegisterDomainDependencies(this IServiceCollection services)
        {
            services.AddTransient<IProductService, ProductService>();

            return services;
        }
    }
}
