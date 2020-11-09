using System.Threading.Tasks;
using MassTransit;
using Mediator.PeriodicTable.Contracts;

namespace RabbitMQ.PeriodicTable.PublisherApp
{
    public class PeriodicTableConsumer :
        IConsumer<GetElement>
    {
        public async Task Consume(ConsumeContext<GetElement> context)
        {
            System.Console.WriteLine("Requested: {0}", context.Message.ElementId);
            await context.RespondAsync<IElement>(ElementService.Instance.GetElementById(context.Message.ElementId));
            System.Console.WriteLine("Responded...");
        }
    }
}
