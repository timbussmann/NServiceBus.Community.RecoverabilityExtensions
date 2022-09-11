using NServiceBus.Community.RecoverabilityExtensions;

namespace DemoEndpoint;

[ImmediateRetries(1)]
[DelayedRetries(1, "0:00:10")]
[ErrorQueue("demo-error")]
public class Handler1 : IHandleMessages<DemoMessage>
{
    public Task Handle(DemoMessage message, IMessageHandlerContext context)
    {
        Console.WriteLine($"Invoked {nameof(Handler1)}.");

        throw new Exception("q_q");
    }
}