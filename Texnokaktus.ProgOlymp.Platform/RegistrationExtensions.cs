using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Hosting;

namespace Texnokaktus.ProgOlymp.Platform;

public static class RegistrationExtensions
{
    public static IHostApplicationBuilder UsePlatform(this WebApplicationBuilder builder, Action<IPlatformConfigurator>? comfigurationAction = null)
    {
        var platformConfigurator = new PlatformConfigurator();
        comfigurationAction?.Invoke(platformConfigurator);
        platformConfigurator.ApplyConfiguration(builder);

        return builder;
    }
}
