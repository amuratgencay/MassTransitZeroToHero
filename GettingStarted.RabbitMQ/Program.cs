using MassTransit;
using MassTransit.RabbitMqTransport;
using System;
using System.Threading.Tasks;

namespace GettingStarted.RabbitMQ
{
    public class Message
    {
        public string Text { get; set; }
    }

    public class Program
    {
        public static async Task Main()
        {
            var bus = Bus.Factory.CreateUsingRabbitMq(ConfigureRabbitMQBus());
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

        private static Action<IRabbitMqBusFactoryConfigurator> ConfigureRabbitMQBus()
        {
            return sbc =>
            {
                sbc.Host("rabbitmq://localhost");

                sbc.ReceiveEndpoint("message_queue", ep =>
                {
                    ep.Handler<Message>(context => 
                        Console.Out.WriteLineAsync($"Received: {context.Message.Text}"));
                });
            };
        }
    }
}
