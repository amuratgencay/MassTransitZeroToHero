using System;
using System.Diagnostics;
using System.Threading.Tasks;
using ExceptionHandling.PeriodicTable.Contracts;
using MassTransit;

namespace ExceptionHandling.PeriodicTable.PublisherApp
{
    public class PeriodicTableConsumer :
        IConsumer<GetElement>
    {
        public async Task Consume(ConsumeContext<GetElement> context)
        {
            Debug.WriteLine("Heelo From Consumer");
            Debug.WriteLine(context);
            Console.WriteLine("Requested: {0}", context.Message.ElementId);
            if (int.TryParse(context.Message.ElementId, out var elementId))
            {
                var element = ElementService.Instance.GetElementById(elementId);
                if (element != null)
                    await context.RespondAsync<ElementAccepted>(element);
                else
                    await context.RespondAsync<ElementRejected>(new
                    {
                        context.Message.ElementId,
                        Reason = "Element Not Found!"
                    });
            }
            else
            {
                throw new ArgumentException("ElementId is not numeric value!"
                    + context.ResponseAddress);
            }
            Console.WriteLine("Responded...");
        }
    }
}
