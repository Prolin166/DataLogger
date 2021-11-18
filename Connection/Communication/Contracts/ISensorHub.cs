using System;

namespace Connection.Communication.Contracts
{
    public interface ISensorHub
    {
        event EventHandler<SensorDataReceivedArgs> SensorDataReceived;
        void Initialize();
        void Connection_WriteStringData(string data);
        void Connection_WriteByteData(byte data);
    }
}