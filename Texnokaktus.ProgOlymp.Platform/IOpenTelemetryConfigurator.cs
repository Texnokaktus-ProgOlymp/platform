using Microsoft.Extensions.DependencyInjection;
using OpenTelemetry.Metrics;
using OpenTelemetry.Trace;
using Texnokaktus.ProgOlymp.OpenTelemetry;

namespace Texnokaktus.ProgOlymp.Platform;

public interface IOpenTelemetryConfigurator
{
    public IOpenTelemetryConfigurator WithTracerConfiguration(Action<TracerProviderBuilder> configurationAction);
    public IOpenTelemetryConfigurator WithMeterConfiguration(Action<MeterProviderBuilder> configurationAction);
}

internal class OpenTelemetryConfigurator : IOpenTelemetryConfigurator
{
    private Action<TracerProviderBuilder>? _tracerConfigurationAction;
    private Action<MeterProviderBuilder>? _meterConfigurationAction;

    public IOpenTelemetryConfigurator WithTracerConfiguration(Action<TracerProviderBuilder> configurationAction)
    {
        _tracerConfigurationAction += configurationAction;
        return this;
    }

    public IOpenTelemetryConfigurator WithMeterConfiguration(Action<MeterProviderBuilder> configurationAction)
    {
        _meterConfigurationAction += configurationAction;
        return this;
    }

    public void ApplyConfiguration(IServiceCollection services, string serviceName) =>
        services.AddTexnokaktusOpenTelemetry(serviceName, _tracerConfigurationAction, _meterConfigurationAction);
}
