using Connection.Protocols;
using Connection.Protocols.Contracts;
using Connection.Protocols.Enums;
using Connection.Protocols.Options;
using System;

namespace Connection.Communication
{
    public class DeviceConnectionFactory
    {
        #region Options
        public DeviceConnectionType DeviceConnectionType { get; set; }
        public SerialConnectionOptions SerialConnectionOptions { get; set; }
        public SimulationConnectionOptions SimulationConnectionOptions { get; set; }
        #endregion

        public DeviceConnection CreateConnection()
        {
            DeviceConnection deviceConnection;
            switch (DeviceConnectionType)
            {
                case DeviceConnectionType.Simulation:
                    deviceConnection = new SimulationConnection(SimulationConnectionOptions);
                    break;
                case DeviceConnectionType.Serial:
                    deviceConnection = new SerialConnection(SerialConnectionOptions);
                    break;
                case DeviceConnectionType.Test:
                    deviceConnection = new TestConnection();
                    break;
                case DeviceConnectionType.None:
                default:
                   throw new NotImplementedException("DeviceConnectionType unknown");
            }

            return deviceConnection;
        }
    }
}
