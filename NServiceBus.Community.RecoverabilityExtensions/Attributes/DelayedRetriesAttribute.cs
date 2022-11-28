namespace NServiceBus.Community.RecoverabilityExtensions;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
public class DelayedRetriesAttribute : Attribute
{
    public DelayedRetriesAttribute(int numberOfRetries)
    {
        NumberOfRetries = numberOfRetries;
        TimeIncrease = null;
    }

    public DelayedRetriesAttribute(int numberOfRetries, string timeIncrease)
    {
        NumberOfRetries = numberOfRetries;
        TimeIncrease = TimeSpan.Parse(timeIncrease);
    }

    public int NumberOfRetries { get; }

    public TimeSpan? TimeIncrease { get; }
}