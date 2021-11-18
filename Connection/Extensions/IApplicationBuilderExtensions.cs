using Connection.Communication;
using Connection.Communication.Contracts;
using Microsoft.AspNetCore.Builder;

namespace Connection.Extensions
{
    public static class IApplicationBuilderExtensions
    {
        public static void UseSensorHub(this IApplicationBuilder app)
        {
            var sensorHub = app.ApplicationServices.GetService(typeof(ISensorHub)) as SensorHub;
            sensorHub.Initialize();
        }
    }
    
}
