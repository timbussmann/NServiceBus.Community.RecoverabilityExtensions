namespace NServiceBus.Community.RecoverabilityExtensions;

public static class ConfigurationExtensions
{
    public static void EnableAttributeAwareRecoverabilityPolicy(
        this EndpointConfiguration endpointConfiguration)
    {
        endpointConfiguration.Pipeline.Register(typeof(HandlerTrackingBehavior), "Captures failing handlers for the AttributeAwareRecoverabilityPolicy");
        var recoverabilityPolicy = new AttributeAwareRecoverabilityPolicy();
        endpointConfiguration.Recoverability().CustomPolicy(recoverabilityPolicy.Invoke);
    }
}