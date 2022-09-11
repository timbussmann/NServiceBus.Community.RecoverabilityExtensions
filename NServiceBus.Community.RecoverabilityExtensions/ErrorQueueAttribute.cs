namespace NServiceBus.Community.RecoverabilityExtensions;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
public class ErrorQueueAttribute : Attribute
{
    public ErrorQueueAttribute(string errorQueue)
    {
        ErrorQueue = errorQueue;
    }

    public string ErrorQueue { get; }
}