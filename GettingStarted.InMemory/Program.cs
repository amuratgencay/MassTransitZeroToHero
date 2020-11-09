using MassTransit;
using System;
using System.Threading.Tasks;

namespace GettingStarted.InMemory
{
    public class Message
    {
        public string Text { get; set; }
    }

    public class Program
    {
        public static async Task Main()
        {
            var bus = Bus.Factory.CreateUsingInMemory(ConfigureInMemoryBus());
            await bus.StartAsync();
            try { await ReadAndPublishMessage(bus); }
            finally { await bus.StopAsync(); }
        }

        private static async Task ReadAndPublishMessage(IBusControl bus)
        {
            do
            {
                string value = await Task.Run(() =>
                {
                    Console.WriteLine("Enter message (or quit to exit)");
                    Console.Write("> ");
                    return Console.ReadLine();
                });

                if ("quit".Equals(value, StringComparison.OrdinalIgnoreCase)) break;
                await bus.Publish(new Message { Text = value });
                await Task.Delay(1000);
            }
            while (true);
        }

        private static Action<IInMemoryBusFactoryConfigurator> ConfigureInMemoryBus()
        {
            return sbc =>
            {
                sbc.ReceiveEndpoint("message_queue", ep =>
                {
                    ep.Handler<Message>(ctx => 
                        Console.Out.WriteLineAsync($"Received: {ctx.Message.Text}"));
                });
            };
        }



    }
}
