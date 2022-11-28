using NServiceBus;
using NServiceBus.AcceptanceTesting.Support;
using NServiceBus.Community.RecoverabilityExtensions;

namespace RecoverabilityExtensions.AcceptanceTests;

class ServerWithoutRetries : IEndpointSetupTemplate
{
    public async Task<EndpointConfiguration> GetConfiguration(RunDescriptor runDescriptor, EndpointCustomizationConfiguration endpointConfiguration,
        Func<EndpointConfiguration, Task> configurationBuilderCustomization)
    {
        var configuration = new EndpointConfiguration(endpointConfiguration.EndpointName);

        configuration.UseTransport<LearningTransport>();
        configuration.UsePersistence<LearningPersistence>();

        configuration.EnableAttributeAwareRecoverabilityPolicy();

        configuration.PurgeOnStartup(true);
        configuration.Recoverability()
            .Immediate(i => i.NumberOfRetries(0))
            .Delayed(d => d.NumberOfRetries(0));

        await configurationBuilderCustomization(configuration);

        return configuration;
    }
}