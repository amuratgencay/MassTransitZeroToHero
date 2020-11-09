using ExceptionHandling.PeriodicTable.Contracts;
using MassTransit;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ExceptionHandling.PeriodicTable.ConsumerApp
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

            Console.WriteLine("Request : Element({0})", value);
            var (accepted, rejected) = await requestClient
                .GetResponse<ElementAccepted, ElementRejected>(new
                {
                    ElementId = value,
                    __FaultAddress = "rabbitmq://localhost/periodic-table-exception-handling-faults"
                });
            if (accepted.IsCompleted)
            {

                var response = await accepted;
                Console.WriteLine("Response: {0}", new Element(response.Message));
            }
            else
            {
                var response = await rejected;
                Console.WriteLine("Response: {0}", response.Message.Reason);
            }
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
