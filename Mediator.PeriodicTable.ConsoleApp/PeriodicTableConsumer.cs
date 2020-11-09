using System.Threading.Tasks;
using MassTransit;
using Mediator.PeriodicTable.Contracts;
using Mediator.PeriodicTable.Service;

namespace Mediator.PeriodicTable.ConsoleApp
{
    public class PeriodicTableConsumer :
        IConsumer<GetElement>
    {
        public async Task Consume(ConsumeContext<GetElement> context)
        {
            await context.RespondAsync<IElement>(
                ElementService.Instance.GetElementById(context.Message.ElementId));
        }
    }
}
