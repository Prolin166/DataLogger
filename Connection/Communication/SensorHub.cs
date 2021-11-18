using System;
using Connection.Communication.Contracts;
using Connection.Protocols.Contracts;
using Connection.Protocols.Events;

namespace Connection.Communication
{
    /// <summary>
    /// Observable Pattern for receciving and sending measurement data to all listeners interested in it
    /// </summary>
    public class SensorHub : ISensorHub
    {
        private readonly DeviceConnection _connection;
        public event EventHandler<SensorDataReceivedArgs> SensorDataReceived;


        public SensorHub(DeviceConnectionFactory deviceConnectionFactory)
        {
            _connection = deviceConnectionFactory.CreateConnection();
        }

        private void Connection_StringReceived(object sender, StringReceivedEventArgs e)
        {
            if (SensorDataReceived != null)
                SensorDataReceived(this, new SensorDataReceivedArgs(stringData: e.Data));
        }           

        private void Connection_ByteArrayReceived(object sender, ByteArrayReceivedEventArgs e)
        {
            if (SensorDataReceived != null)
                SensorDataReceived(this, new SensorDataReceivedArgs(rawData: e.Data));
        }

        public void Initialize()
        {
            _connection.ByteArrayReceived += Connection_ByteArrayReceived;
            _connection.StringReceived += Connection_StringReceived;
            
            _connection.Connect();

        }

        public void Connection_WriteStringData(string data)
        {
            _connection.Write(data);
        }

        public void Connection_WriteByteData(byte data)
        {
            _connection.Write(data);
        }
    }
}