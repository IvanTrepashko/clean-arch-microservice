using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WarehouseService.Domain.Abstractions;
using WarehouseService.Infrastructure.Abstractions;
using WarehouseService.Infrastructure.Data;
using WarehouseService.Infrastructure.Data.Repositories;

namespace WarehouseService.Infrastructure
{
    public static class DefaultInfrastructureModule
    {
        public static IServiceCollection AddInfrastructureDependencied(this IServiceCollection services, IConfigurationSection configuration)
        {
            AddRepositories(services);

            return services;
        }

        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services.AddTransient<ICategoryRepository, CategoryRepository>();
            services.AddTransient<IProductRepository, ProductRepository>();
            services.AddTransient<IOrderRepository, OrderRepository>();

            services.AddTransient<IIdGenerator, IdGenerator>();

            return services;
        }
    }
}
