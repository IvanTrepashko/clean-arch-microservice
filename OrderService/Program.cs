using MassTransit;
using MassTransit.Registration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace OrderService
{
    public class Program
    {
        public static void Main()
        {
            var sc = new ServiceCollection();

            sc.AddLogging(c =>
            {
                c.AddConsole();
            });

            sc.AddMassTransit(c =>
            {
                c.Registrar.RegisterConsumer<ApproveOrderConsumer>();

                c.AddConsumer<ApproveOrderConsumer>();

                c.UsingRabbitMq((context, cfg) =>
                {
                    cfg.Host("amqp://guest:guest@localhost:5672/");

                    cfg.ConfigureEndpoints(context);
                });
            });

            var provider = sc.BuildServiceProvider();
            var depot = provider.GetService<IBusDepot>();
            var bus = provider.GetService<IBus>();

            Task.Run(() => depot.Start(CancellationToken.None));

            Thread.Sleep(Timeout.Infinite);
        }
    }
}