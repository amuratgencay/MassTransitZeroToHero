using MassTransit;
using Mediator.PeriodicTable.Contracts;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace RabbitMQ.PeriodicTable.ConsumerApp
{
    class PeriodicTableConsumerApp
    {
        public static async Task Main()
        {
            var busControl = Bus.Factory.CreateUsingRabbitMq();
            var source = new CancellationTokenSource(TimeSpan.FromSeconds(10));
            await busControl.StartAsync(source.Token);
            try
            {
                IRequestClient<GetElement> requestClient = busControl.CreateRequestClient<GetElement>();
                do
                {
                    string value = await GetElementNumberFromUser();
                    if ("quit".Equals(value, StringComparison.OrdinalIgnoreCase)) break;
                    await GetElementFromUserInput(requestClient, value);
                    await Task.Delay(1000);
                }
                while (true);
            }
            finally
            {
                await busControl.StopAsync();
            }

        }

        private static async Task GetElementFromUserInput(IRequestClient<GetElement> requestClient, string value)
        {
            if (int.TryParse(value, out var elementId))
            {
                Console.WriteLine("Request : Element({0})", elementId);
                Response<IElement> response = await requestClient
                    .GetResponse<IElement>(new { ElementId = elementId });
                Console.WriteLine("Response: {0}", new Element(response.Message));
            }
            else
                Console.WriteLine("Wrong number!!!");
        }

        private static async Task<string> GetElementNumberFromUser()
        {
            return await Task.Run(() =>
            {
                Console.WriteLine("Enter Element Number (or quit to exit)");
                Console.Write("> ");
                return Console.ReadLine();
            });
        }
    }
}
