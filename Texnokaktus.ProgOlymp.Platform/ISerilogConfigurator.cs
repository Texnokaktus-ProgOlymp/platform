using Microsoft.Extensions.Hosting;
using Serilog;

namespace Texnokaktus.ProgOlymp.Platform;

public interface ISerilogConfigurator
{
    public ISerilogConfigurator ConfigureSerilog(Action<HostBuilderContext, LoggerConfiguration> configurationAction);
}

internal class SerilogConfigurator : ISerilogConfigurator
{
    private const string OutputTemplate = "{Timestamp:HH:mm:ss.fff K} [{Level:u3}] <{SourceContext}> {Message:lj} {NewLine}{Exception}";

    private Action<HostBuilderContext, LoggerConfiguration> _serilogConfigurationAction = (context, configuration) =>
        configuration.ReadFrom.Configuration(context.Configuration)
                     .MinimumLevel.Information()
                     .MinimumLevel.Override("Microsoft", Serilog.Events.LogEventLevel.Warning)
                     .MinimumLevel.Override("Microsoft.Hosting.Lifetime", Serilog.Events.LogEventLevel.Information)
                     .WriteTo.Console(outputTemplate: OutputTemplate)
                     .Enrich.FromLogContext()
                     .Enrich.WithThreadId()
                     .Enrich.WithExceptionData()
                     .Enrich.WithEnvironmentName()
                     .Enrich.WithAssemblyName()
                     .Enrich.WithMachineName();

    public ISerilogConfigurator ConfigureSerilog(Action<HostBuilderContext, LoggerConfiguration> configurationAction)
    {
        _serilogConfigurationAction += configurationAction;
        return this;
    }

    public void ApplyConfiguration(IHostBuilder hostBuilder) => hostBuilder.UseSerilog(_serilogConfigurationAction);
}
