using DemoEndpoint;
using NServiceBus.Community.RecoverabilityExtensions;

var endpointConfiguration = new EndpointConfiguration("DemoEndpoint");
endpointConfiguration.UseTransport<LearningTransport>();

var recoverabilitySettings = endpointConfiguration.Recoverability();
recoverabilitySettings.Immediate(c => c.NumberOfRetries(5));

endpointConfiguration.EnableAttributeAwareRecoverabilityPolicy();

endpointConfiguration.PurgeOnStartup(true);
var endpoint = await Endpoint.Start(endpointConfiguration);
await endpoint.SendLocal(new DemoMessage());
Console.WriteLine("Press any key to exit");
Console.ReadKey();
await endpoint.Stop();
