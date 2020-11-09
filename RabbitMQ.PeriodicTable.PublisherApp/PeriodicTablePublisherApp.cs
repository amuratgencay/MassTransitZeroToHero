using MassTransit;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace RabbitMQ.PeriodicTable.PublisherApp
{
    class PeriodicTablePublisherApp
    {
        static async Task Main()
        {
            var busControl = Bus.Factory.CreateUsingRabbitMq(cfg =>
            {
                cfg.ReceiveEndpoint("periodic-table", e =>
                {
                    e.Consumer<PeriodicTableConsumer>();
                });
            });
            var source = new CancellationTokenSource(TimeSpan.FromSeconds(10));

            await busControl.StartAsync(source.Token);
            try
            {
                Console.WriteLine("Press enter to exit");
                await Task.Run(() => Console.ReadLine());
            }
            finally
            {
                await busControl.StopAsync();
            }

        }
    }
}
