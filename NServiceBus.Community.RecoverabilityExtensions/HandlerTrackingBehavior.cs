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
            var handlerType = context.MessageHandler.HandlerType;
            e.Data.Add(HandlerTypeExceptionDataKey, handlerType);
            throw;
        }
    }
}