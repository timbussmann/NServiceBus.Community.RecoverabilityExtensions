using NServiceBus.AcceptanceTesting;
using NServiceBus.Community.RecoverabilityExtensions;
using NServiceBus;

namespace RecoverabilityExtensions.AcceptanceTests;

public class When_using_delayed_retries_attribute
{
    [Test]
    public async Task Should_apply_attribute_configuration()
    {
        var context = await Scenario.Define<Context>()
            .WithEndpoint<EndpointWithDelayedRetries>(e => e
                .DoNotFailOnErrorMessages()
                .When(s => s.SendLocal(new TestMessage())))
            .Done(c => c.FailedMessages.Any())
            .Run();

        Assert.AreEqual(6, context.HandlerInvocations, "should invoke the handler 1 + number of configured retries on the attribute");
        var failedMessage = context.FailedMessages.Single().Value.Single();
        Assert.AreEqual(5, int.Parse(failedMessage.Headers[Headers.DelayedRetries]));
    }

    class Context : ScenarioContext
    {
        public int HandlerInvocations { get; set; }
    }

    class EndpointWithDelayedRetries : EndpointConfigurationBuilder
    {
        public EndpointWithDelayedRetries()
        {
            EndpointSetup<ServerWithoutRetries>();
        }

        [DelayedRetries(5, "00:00:01")]
        class TestHandler : IHandleMessages<TestMessage>
        {
            private readonly Context testContext;

            public TestHandler(Context testContext)
            {
                this.testContext = testContext;
            }

            public Task Handle(TestMessage message, IMessageHandlerContext context)
            {
                testContext.HandlerInvocations++;
                throw new Exception("qq");
            }
        }
    }

    class TestMessage : IMessage
    {
    }
}