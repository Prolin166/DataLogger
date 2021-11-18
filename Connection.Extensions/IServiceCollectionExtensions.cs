using Connection.Communication;
using Connection.Communication.Contracts;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Connection.Extensions
{
    public static class IServiceCollectionExtensions
    {
        public static void AddSensorHub(this IServiceCollection services, IConfiguration configuration)
        {
            var deviceConnectionFactory = new DeviceConnectionFactory();
            configuration.GetSection("DeviceConnection").Bind(deviceConnectionFactory);

            services.AddSingleton(deviceConnectionFactory);
            services.AddSingleton<ISensorHub, SensorHub>();


        }
    }
}
