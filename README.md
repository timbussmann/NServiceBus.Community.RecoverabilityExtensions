# NServiceBus.Community.RecoverabilityExtensions

an NServiceBus extension that provides recoverability customizations for the recoverability behavior of individual handlers via attributes.

Use the `ImmediateRetries`, `DelayedRetries`, and `ErrorQueue` attributes on a class that implements `IHandleMessages<T>` to customize the recoverability behavior in case the handler fails throws an exception.

See the `DemoEndpoint` project in this repository for a simple demo.
