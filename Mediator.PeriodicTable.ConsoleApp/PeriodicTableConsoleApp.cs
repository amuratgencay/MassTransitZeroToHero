using System;
using System.Threading.Tasks;
using MassTransit;
using MassTransit.Mediator;
using Mediator.PeriodicTable.Contracts;
using Microsoft.Extensions.DependencyInjection;
namespace Mediator.PeriodicTable.ConsoleApp
{
    public class PeriodicTableConsoleApp
    {
        public static async Task Main()
        {
            do
            {
                string value = await GetElementNumberFromUser();
                if ("quit".Equals(value, StringComparison.OrdinalIgnoreCase)) break;
                await GetElementFromUserInput(value);
                await Task.Delay(1000);
            }
            while (true);
        }

        private static async Task GetElementFromUserInput(string value)
        {
            if (int.TryParse(value, out var elementId))
            {
                Console.WriteLine("Request : Element({0})", elementId);

                var response = await GetRequestClient<GetElement>()
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

        private static IRequestClient<T> GetRequestClient<T>() where T : class
            => ConfigureMediator().CreateRequestClient<T>();

        private static IMediator ConfigureMediator()
        {
            var services = new ServiceCollection();

            services.AddMediator(cfg =>
            {
                cfg.AddConsumer<PeriodicTableConsumer>();
            });

            var provider = services.BuildServiceProvider();

            var mediator = provider.GetRequiredService<IMediator>();
            return mediator;
        }
    }
}
