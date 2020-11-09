using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using ExceptionHandling.PeriodicTable.Contracts;
using MassTransit;

namespace ExceptionHandling.PeriodicTable.PublisherApp
{
    public class PeriodicTableFaultConsumer :
        IConsumer<Fault<GetElement>>
    {
        public async Task Consume(ConsumeContext<Fault<GetElement>> context)
        {
            var exceptionMessage = "ElementId is not numeric value!";
            var exception = context.Message.Exceptions.FirstOrDefault(x => x.Message.Contains(exceptionMessage));
            if (exception != null)
            {
                Debug.WriteLine("Heelo From Fault");
                Debug.WriteLine(exception.Message.Replace(exceptionMessage, ""));
                Console.WriteLine("Fault Consumer -> Requested: {0}", context.Message.Message.ElementId);
                var element = ElementService.Instance.GetElementByName(context.Message.Message.ElementId);
                Console.WriteLine(element);
                
                string uriString = exception.Message.Replace(exceptionMessage, "");
                if (string.IsNullOrEmpty(uriString))
                    uriString = context.DestinationAddress.ToString();

                ISendEndpoint sendEndpoint = await context.GetSendEndpoint(
                    new Uri(uriString));

                if (element != null)
                    await context.Publish<ElementAccepted>(element);
                else
                    await context.Publish<ElementRejected>(new
                    {
                        context.Message.Message.ElementId,
                        Reason = "Element Not Found!"
                    });
                Console.WriteLine("Responded...");
            }
            
        }
    }
}
