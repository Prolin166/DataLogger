using Business.Consumers.Contracts;
using Connection.Communication;
using Connection.Communication.Contracts;
using System;
using System.Text;

namespace Business.Consumers
{
    public class ConsoleMeasurementHandler : IConsoleMeasurementHandler
    {

        private readonly ISensorHub _sensorHub;

        public ConsoleMeasurementHandler(ISensorHub sensorHub)
        {
            _sensorHub = sensorHub;
        }

        private void HandleSensorData(object sender, SensorDataReceivedArgs e)
        {
            if (e.StringReceived)
            {
                Console.WriteLine($" StringDataReceived: { e.StringData }");
            }

            if (e.RawDataReceived)
            {
                Console.WriteLine($" RawDataReceived: { new ASCIIEncoding().GetString(e.RawData) }" );
            }
        }

        public void StartHandler()
        {
                _sensorHub.SensorDataReceived += HandleSensorData;
        }

        public void StopHandler()
        {
            _sensorHub.SensorDataReceived -= HandleSensorData;
        }
    }
}
