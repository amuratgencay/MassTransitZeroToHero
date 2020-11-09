using ExceptionHandling.PeriodicTable.Contracts;
using ExceptionHandling.PeriodicTable.PublisherApp;
using MassTransit;
using MassTransit.Testing;
using NUnit.Framework;
using System.Linq;
using System.Threading.Tasks;

namespace PeriodicTable.Tests
{
    [TestFixture]
    public class Tests
    {
        #region Inits
        private InMemoryTestHarness _harness;
        private ConsumerTestHarness<PeriodicTableConsumer> _consumerHarness;
        private ConsumerTestHarness<PeriodicTableFaultConsumer> _faultConsumerHarness;

        [SetUp]
        public async Task SetupAsync()
        {
            _harness = new InMemoryTestHarness();
            _consumerHarness = _harness.Consumer<PeriodicTableConsumer>();
            _faultConsumerHarness = _harness.Consumer<PeriodicTableFaultConsumer>();
            await _harness.Start();
        }
        [TearDown]
        public async Task TearDownAsync()
        {
            await _harness.Stop();
        }
        #endregion
       
        #region Utils
        private async Task IsConsumersWorked(bool faultCase = false)
        {
            // did the endpoint consume the message
            Assert.That(await _harness.Consumed.Any<GetElement>());

            // did the actual consumer consume the message
            Assert.That(await _consumerHarness.Consumed.Any<GetElement>());

            // did the fault consumer consume the message
            if (faultCase)
                Assert.That(await _faultConsumerHarness.Consumed.Any<Fault<GetElement>>(), Is.True);
            else
                Assert.That(await _faultConsumerHarness.Consumed.Any<Fault<GetElement>>(), Is.False);
        }
        private async Task IsEventsPublished(bool accepted = true, bool faultCase = false)
        {

            // the consumer publish the event
            if (accepted)
                Assert.That(await _harness.Published.Any<ElementAccepted>(), Is.True);
            else
                Assert.That(await _harness.Published.Any<ElementAccepted>(), Is.False);

            // the consumer publish the event
            if (!accepted)
                Assert.That(await _harness.Published.Any<ElementRejected>(), Is.True);
            else
                Assert.That(await _harness.Published.Any<ElementRejected>(), Is.False);

            // ensure that no faults were published by the consumer
            if (faultCase)
                Assert.That(await _harness.Published.Any<Fault<GetElement>>(), Is.True);
            else
                Assert.That(await _harness.Published.Any<Fault<GetElement>>(), Is.False);
        }
        private static void IsElementRejectedEquals(ElementRejected elementRejected, string elementId, string reason)
        {
            Assert.That(elementRejected, Is.Not.Null);
            Assert.That(elementRejected.ElementId, Is.EqualTo(elementId));
            Assert.That(elementRejected.Reason, Is.EqualTo(reason));
        }
        private static void IsElementAcceptedEquals(ElementAccepted elementAccepted, string elementId, string shortName, string longName)
        {
            Assert.That(elementAccepted, Is.Not.Null);
            Assert.That(elementAccepted.ElementId, Is.EqualTo(elementId));
            Assert.That(elementAccepted.ShortName, Is.EqualTo(shortName));
            Assert.That(elementAccepted.LongName, Is.EqualTo(longName));
        }
        #endregion

        #region Tests
        [Test]
        public async Task Should_publish_the_element_accepted_event()
        {
            await _harness.InputQueueSendEndpoint
                .Send<GetElement>(new { ElementId = "11" });

            await IsConsumersWorked(faultCase: false);
            await IsEventsPublished(accepted: true, faultCase: false);
        }
        [Test]
        public async Task Should_publish_the_element_rejected_event()
        {
            await _harness.InputQueueSendEndpoint
                .Send<GetElement>(new { ElementId = "130" });

            await IsConsumersWorked(faultCase: false);
            await IsEventsPublished(accepted: false, faultCase: false);
        }
        [Test]
        public async Task Should_publish_the_element_accepted_event_after_fault()
        {
            await _harness.InputQueueSendEndpoint
                .Send<GetElement>(new { ElementId = "Na" });

            await IsConsumersWorked(faultCase: true);
            await IsEventsPublished(accepted: true, faultCase: true);
        }
        [Test]
        public async Task Should_publish_the_element_rejected_event_after_fault()
        {
            await _harness.InputQueueSendEndpoint
                .Send<GetElement>(new { ElementId = "Mm" });

            await IsConsumersWorked(faultCase: true);
            await IsEventsPublished(accepted: false, faultCase: true);
        }
        [Test]
        public async Task Should_Element_Not_Found_return_when_send_130()
        {
            var requestClient = _harness.CreateRequestClient<GetElement>();
            var (accepted, rejected) = await requestClient
                .GetResponse<ElementAccepted, ElementRejected>(new { ElementId = 130 });

            await IsConsumersWorked();

            //the consumer not publish the element accepted event
            Assert.True(!accepted.IsCompleted || accepted.IsCanceled);

            //the consumer publish the element rejected event
            Assert.True(rejected.IsCompleted);

            var response = await rejected;

            //response filled by error message
            IsElementRejectedEquals(response.Message, "130", "Element Not Found!");
        }
        [Test]
        public async Task Should_Na_return_when_send_11()
        {
            var requestClient = _harness.CreateRequestClient<GetElement>();
            var (accepted, rejected) = await requestClient
             .GetResponse<ElementAccepted, ElementRejected>(new { ElementId = 11 });

            await IsConsumersWorked();

            //the consumer publish the element accepted event
            Assert.True(accepted.IsCompleted);

            //the consumer not publish the element rejected event
            Assert.True(!rejected.IsCompleted || rejected.IsCanceled);

            var response = await accepted;

            //response filled by Sodium Element
            IsElementAcceptedEquals(response.Message, "11", "Na", "Sodium");
        }
        [Test]
        public async Task Should_Sodium_return_when_send_Na()
        {
            await _harness.InputQueueSendEndpoint
                .Send<GetElement>(new { ElementId = "Na" });

            await IsConsumersWorked(faultCase: true);
            await IsEventsPublished(accepted: true, faultCase: true);

            // get published messages from fault consumer
            var publishedMessage = _harness.Published.Select<ElementAccepted>()
                .ToList().FirstOrDefault();

            //message has Sodium Element
            IsElementAcceptedEquals(publishedMessage.Context.Message, "11", "Na", "Sodium");

        }
        [Test]
        public async Task Should_Element_Not_Found_return_when_send_Mm()
        {
            await _harness.InputQueueSendEndpoint
                .Send<GetElement>(new { ElementId = "Mm" });
            await IsConsumersWorked(faultCase: true);
            await IsEventsPublished(accepted: false, faultCase: true);

            // get published messages from fault consumer
            var publishedMessage = _harness.Published.Select<ElementRejected>()
                .ToList().FirstOrDefault();

            //message has Element Not Found! exception
            IsElementRejectedEquals(publishedMessage.Context.Message, "Mm", "Element Not Found!");

        }

        #endregion
    }
}