using System.Reflection;
using NServiceBus.Transport;

namespace NServiceBus.Community.RecoverabilityExtensions;

public class AttributeAwareRecoverabilityPolicy
{
    public RecoverabilityAction Invoke(RecoverabilityConfig recoverabilityConfig, ErrorContext errorContext)
    {
        var failingHandlerType = GetHandlerTypeFromExceptionData(errorContext.Exception);
        if (failingHandlerType == null)
        {
            // Exception happened earlier in the pipeline so execute the default recoverability behavior
            return DefaultRecoverabilityPolicy.Invoke(recoverabilityConfig, errorContext);
        }

        ImmediateConfig immediateConfig = recoverabilityConfig.Immediate;
        DelayedConfig delayedConfig = recoverabilityConfig.Delayed;
        

        bool hasAttributes = false;
        if (failingHandlerType.GetCustomAttribute<ImmediateRetriesAttribute>() is { } immediateRetriesAttribute)
        {
            immediateConfig = new ImmediateConfig(immediateRetriesAttribute.NumberOfRetries);
            hasAttributes = true;
        }

        if (failingHandlerType.GetCustomAttribute<DelayedRetriesAttribute>() is { } delayedRetriesAttribute)
        {
            delayedConfig = new DelayedConfig(delayedRetriesAttribute.NumberOfRetries, delayedRetriesAttribute.TimeIncrease ?? delayedConfig.TimeIncrease);
            hasAttributes = true;
        }

        FailedConfig failedConfig;
        if (failingHandlerType.GetCustomAttribute<ErrorQueueAttribute>() is { } errorQueueAttribute)
        {
            failedConfig = new FailedConfig(errorQueueAttribute.ErrorQueue, new HashSet<Type>(0));
        }
        else if(hasAttributes)
        {
            // ignore unrecoverable exception setting if attributes are set on this handler
            failedConfig = new FailedConfig(recoverabilityConfig.Failed.ErrorQueue, new HashSet<Type>(0));
        }
        else
        {
            failedConfig = recoverabilityConfig.Failed;
        }

        return DefaultRecoverabilityPolicy.Invoke(new RecoverabilityConfig(immediateConfig, delayedConfig, failedConfig), errorContext);
    }

    private Type? GetHandlerTypeFromExceptionData(Exception errorContextException)
    {
        if (errorContextException.Data.Contains(HandlerTrackingBehavior.HandlerTypeExceptionDataKey))
        {
            return errorContextException.Data[HandlerTrackingBehavior.HandlerTypeExceptionDataKey] as Type;
        }

        return errorContextException.InnerException != null ? GetHandlerTypeFromExceptionData(errorContextException.InnerException) : null;
    }
}