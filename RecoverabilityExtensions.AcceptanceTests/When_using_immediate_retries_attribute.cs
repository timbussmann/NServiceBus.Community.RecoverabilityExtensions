using NServiceBus;
using NServiceBus.AcceptanceTesting;
using NServiceBus.Community.RecoverabilityExtensions;

namespace RecoverabilityExtensions.AcceptanceTests;

public class When_using_immediate_retries_attribute
{
    [Test]
    public async Task Should_apply_attribute_configuration()
    {
        var context = await Scenario.Define<Context>()
            .WithEndpoint<EndpointWithImmediateRetries>(e => e
                .DoNotFailOnErrorMessages()
                .When(s => s.SendLocal(new TestMessage())))
            .Done(c => c.FailedMessages.Any())
            .Run();

        Assert.AreEqual(11, context.HandlerInvocations, "should invoke the handler 1 + number of configured retries on the attribute");
        Assert.AreEqual(1, context.FailedMessages.Single().Value.Count);
    }

    class Context : ScenarioContext
    {
        public int HandlerInvocations { get; set; }
    }

    class EndpointWithImmediateRetries : EndpointConfigurationBuilder
    {
        public EndpointWithImmediateRetries()
        {
            EndpointSetup<ServerWithoutRetries>();
        }

        [ImmediateRetries(10)]
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