namespace NServiceBus.Community.RecoverabilityExtensions;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
public class ImmediateRetriesAttribute : Attribute
{
    public ImmediateRetriesAttribute(int numberOfRetries)
    {
        NumberOfRetries = numberOfRetries;
    }

    public int NumberOfRetries { get; }
}