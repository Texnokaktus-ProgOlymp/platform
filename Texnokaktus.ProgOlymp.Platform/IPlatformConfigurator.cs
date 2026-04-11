using Microsoft.AspNetCore.Builder;
using Texnokaktus.ProgOlymp.OpenTelemetry;

namespace Texnokaktus.ProgOlymp.Platform;

public interface IPlatformConfigurator
{
    public IOpenTelemetryConfigurator ConfigureOpenTelemetry { get; }
    public ISerilogConfigurator ConfigureSerilog { get; }
}

internal class PlatformConfigurator : IPlatformConfigurator
{
    private const string ServiceNameVariable = "SERVICE_NAME";

    private readonly OpenTelemetryConfigurator _openTelemetryConfigurator = new();
    private readonly SerilogConfigurator _serilogConfigurator = new();

    public IOpenTelemetryConfigurator ConfigureOpenTelemetry => _openTelemetryConfigurator;
    public ISerilogConfigurator ConfigureSerilog => _serilogConfigurator;

    public void ApplyConfiguration(WebApplicationBuilder webApplicationBuilder)
    {
        var serviceName = Environment.GetEnvironmentVariable(ServiceNameVariable)
                       ?? throw new InvalidOperationException($"'{ServiceNameVariable}' environment variable must be set");

        _openTelemetryConfigurator.ApplyConfiguration(webApplicationBuilder.Services, serviceName);

        _serilogConfigurator.ConfigureSerilog((_, configuration) => configuration.AddOpenTelemetrySupport(serviceName));
        _serilogConfigurator.ApplyConfiguration(webApplicationBuilder.Host);
    }
}
