using MassTransit;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WarehouseService.Application.Integration.Consumer;
using WarehouseService.Application.Integration.Sagas;
using WarehouseService.Application.Integration.Sagas.States;

namespace WarehouseService.Application
{
    public static class DefaultApplicationModule
    {
        public static void AddRequestHandlers(this IServiceCollection services)
        {
            services.AddMediatR(typeof(DefaultApplicationModule).Assembly);
        }

        public static void AddMassTransitWithMongoDb(this IServiceCollection services, IConfiguration configuration)
        {
            var mongo = configuration.GetSection("MongoDbOptions");
            var rabbitmq = configuration.GetSection("RabbitMqOptions");
            string mongoConnectionString = $"mongodb://root:root@localhost:27017";
            string rabbitMqConnectionString = $"amqp://guest:guest@localhost:5672";

            services.AddMassTransit(config =>
            {
                config.Registrar.RegisterConsumer<OrderApprovedConsumer>();
                config.Registrar.RegisterConsumer<StockChangedConsumer>();
                config.Registrar.RegisterConsumer<OrderDeclinedConsumer>();

                config.AddConsumer<OrderApprovedConsumer>();
                config.AddConsumer<StockChangedConsumer>();
                config.AddConsumer<OrderDeclinedConsumer>();

                config.AddSagaStateMachine<OrderSaga, OrderSagaState>(config =>
                {
                    config.UseInMemoryOutbox();
                }).MongoDbRepository(mongoConnectionString, x =>
                    {
                        x.CollectionName = "OrderSaga";
                        x.DatabaseName = "Warehouse";
                    });

                config.UsingRabbitMq((context, cfg) =>
                {
                    cfg.Host(rabbitMqConnectionString);

                    cfg.ConfigureEndpoints(context);
                });
            });
        }
    }
}
