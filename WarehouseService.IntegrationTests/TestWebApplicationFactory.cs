using MassTransit;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System.Linq;
using WarehouseService.API.Options;
using WarehouseService.Infrastructure;
using WarehouseService.Infrastructure.Options;

namespace WarehouseService.IntegrationTests
{
    public class TestWebApplicationFactory<TStartup> : WebApplicationFactory<TStartup>
        where TStartup : class
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                //var configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();

                //var rabbitMqOptions = services.FirstOrDefault(x => x.ServiceType == typeof(IOptions<RabbitMqOptions>));
                //var mongoOptions = services.FirstOrDefault(x => x.ServiceType == typeof(IOptions<MongoDbOptions>));

                //if (rabbitMqOptions != null)
                //{
                //    services.Remove(rabbitMqOptions);
                //}

                //if (mongoOptions != null)
                //{
                //    services.Remove(mongoOptions);
                //}

                //services.Configure<RabbitMqOptions>(configuration.GetSection("RabbitMqOptions_IntegrationTest"));
                //services.Configure<MongoDbOptions>(configuration.GetSection("MongoDbOptions_IntegrationTest"));
            });
        }
    }
}
