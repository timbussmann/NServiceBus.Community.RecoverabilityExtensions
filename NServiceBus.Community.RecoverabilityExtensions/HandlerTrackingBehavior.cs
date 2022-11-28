using NServiceBus.Pipeline;

namespace NServiceBus.Community.RecoverabilityExtensions;

class HandlerTrackingBehavior : Behavior<IInvokeHandlerContext>
{
    internal const string HandlerTypeExceptionDataKey = "RecoverabilityExtensions.FailingHandlerType";

    public override async Task Invoke(IInvokeHandlerContext context, Func<Task> next)
    {
        try
        {
            await next();
        }
        catch (Exception e)
        {
            // This is one of the few possibilities to float data from the pipeline to the error context
            //TODO: this approach only takes the failing handler's config into account. A more sophisticated approach should take into account the config of all invoked handlers so far and select the "lowest" applicable configuration.
            var handlerType = context.MessageHandler.HandlerType;
            e.Data.Add(HandlerTypeExceptionDataKey, handlerType);
            throw;
        }
    }
}