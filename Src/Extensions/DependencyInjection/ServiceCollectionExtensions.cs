using System.Net;
using Microsoft.Extensions.DependencyInjection;

namespace EWeLink.Cube.Api.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddEWeLinkCubeServices(this IServiceCollection services)
        {
            services.AddScoped<ILinkFactory, LinkFactory>();
            services.AddScoped<IDeviceCache, DeviceCache>();
            /*services.AddScoped<ILinkAuthorization>(sc => (ILinkAuthorization)sc.GetRequiredService<ILink>());
            services.AddScoped<Lazy<ILink>>(sc => new Lazy<ILink>(() => sc.GetRequiredService<ILink>(), LazyThreadSafetyMode.PublicationOnly));
            services.AddScoped<ILinkWebSocket, LinkWebSocket>();
            services.AddScoped<ILinkLanControl, LinkLanControl>();*/
            services.AddHttpClient();

            return services;
        }
    }
}